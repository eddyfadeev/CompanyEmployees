using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public record UserForAuthenticationDto
{
    [Required(ErrorMessage = "User name is required!")]
    public string? Username { get; init; }
    [Required(ErrorMessage = "Password is required!")]
    public string? Password { get; init; }
}