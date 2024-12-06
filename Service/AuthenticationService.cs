using Contracts.Logging;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DTO;
using Shared.Extensions;

namespace Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ILoggerManager loggerManager, UserManager<User> userManager,
        IConfiguration configuration)
    {
        _logger = loggerManager;
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = userForRegistration.MapToEntity();

        var result = await _userManager.CreateAsync(user, userForRegistration.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        }

        return result;
    }
}