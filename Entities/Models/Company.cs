using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public sealed class Company : IEquatable<Company>
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

    public bool Equals(Company? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
               Name == other.Name &&
               Address == other.Address &&
               Country == other.Country;
    }
    
    public override bool Equals(object? obj) =>
        Equals(obj as Company);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Name, Address, Country);
}