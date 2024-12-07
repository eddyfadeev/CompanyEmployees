using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Contracts.Logging;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DTO;
using Shared.Extensions;

namespace Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly JwtConfiguration _jwtConfiguration;

    private User? _user;

    public AuthenticationService(ILoggerManager loggerManager, UserManager<User> userManager,
        IConfiguration configuration)
    {
        _logger = loggerManager;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = new JwtConfiguration();
        _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
    }
    
    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = userForRegistration.MapToEntity();

        var result = await _userManager.CreateAsync(user, userForRegistration.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles!);
        }

        return result;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication)
    {
        _user = await _userManager.FindByNameAsync(userForAuthentication.Username!);

        var result =
            (_user is not null && await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password!));

        if (!result)
        {
            _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password.");
        }

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
        {
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }

        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<TokenDto> RefreshToken(TokenDto token)
    {
        var principal = GetPrincipalFromExpiredToken(token.AccessToken);

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);
        if (user is null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenBadRequestException();
        }

        _user = user;

        return await CreateToken(populateExp: false);
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
            ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token.");
        }

        return principal;
    }
}