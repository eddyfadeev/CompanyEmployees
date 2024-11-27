using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public sealed class Employee
{
    [Column("EmployeeId")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Employee name is required.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the name is 60 characters.")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Age is required.")]
    [Range(1, 199, ErrorMessage = "Age must be between 1 and 199.")]
    public int Age { get; set; }
    
    [Required(ErrorMessage = "Position is required.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters.")]
    public string? Position { get; set; }
    
    [ForeignKey(nameof(Company))]
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Employee employee = (Employee)obj;
        return Id == employee.Id && 
               Name == employee.Name && 
               Age == employee.Age && 
               Position == employee.Position && 
               CompanyId == employee.CompanyId;
    }

    protected bool Equals(Employee other)
    {
        return Id.Equals(other.Id) && 
               Name == other.Name && 
               Age == other.Age && 
               Position == other.Position && 
               CompanyId.Equals(other.CompanyId) && 
               Equals(Company, other.Company);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Age, Position, CompanyId, Company);
    }
}