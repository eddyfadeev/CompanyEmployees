using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Company
{
    private ICollection<Employee>? _employees = [];
    
    [Column("CompanyId")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Company name is required.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the name is 60 characters.")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Company address is required.")]
    [MaxLength(200, ErrorMessage = "Maximum length is 200 characters.")]
    public string? Address { get; set; }
        
    public string? Country { get; set; }

    public ICollection<Employee>? Employees
    {
        get => _employees;
        set => _employees = value ?? new List<Employee>();
    }
}