using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public abstract record CompanyForManipulationDto
{
    private readonly IEnumerable<EmployeeForCreationDto>? _employees;
    private readonly string? _country;
    
    [Required(ErrorMessage = "Company name is required.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the name is 60 characters.")]
    public string Name { get; init; } 
    
    [Required(ErrorMessage = "Company address is required.")]
    [MaxLength(200, ErrorMessage = "Maximum length is 200 characters.")]
    public string Address { get; init; }


    public string? Country
    {
        get => _country;
        init => _country = value ?? string.Empty;
    }

    public IEnumerable<EmployeeForCreationDto>? Employees
    {
        get => _employees;
        init => _employees = value ?? [];
    }
}