using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
}