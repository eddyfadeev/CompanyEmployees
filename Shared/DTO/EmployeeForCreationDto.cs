using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public record EmployeeForCreationDto
{
    [Required(ErrorMessage = "Employee name is required.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
    public string Name { get; init; }
    
    [Required(ErrorMessage = "Age is required.")]
    [Range(1, 199, ErrorMessage = "Age must be between 1 and 199.")]
    public int Age { get; init; }
    
    [Required(ErrorMessage = "Position is required.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters.")]
    public string Position { get; init; }
}