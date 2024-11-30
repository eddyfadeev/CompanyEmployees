using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public sealed class Employee : IEquatable<Employee>
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
    
    public bool Equals(Employee? other)
    {
        if (other is null)
        {
            return false;
        }
        
        return Id == other.Id && 
               Name == other.Name && 
               Age == other.Age && 
               Position == other.Position && 
               CompanyId == other.CompanyId;
    }
    
    public override bool Equals(object? obj) =>
        Equals(obj as Employee);

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Age, Position, CompanyId);
    }
}