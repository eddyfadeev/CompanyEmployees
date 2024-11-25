using Entities.Models;
using Shared.DTO;

namespace Shared.Extensions;

public static class MapperExtensions
{
    public static CompanyDto MapToDto(this Company company) =>
        new
        (
            id: company.Id,
            name: company.Name ?? string.Empty,
            address: company.Address ?? string.Empty,
            country: company.Country ?? string.Empty
        );
    
    public static EmployeeDto MapToDto(this Employee employee) =>
        new ()
        {
            Id = employee.Id,
            Name = employee.Name ?? string.Empty,
            Age = employee.Age,
            Position = employee.Position ?? string.Empty
        };
    
    public static Company MapToEntity(this CompanyDto company)
    {
        company.Deconstruct(out var id, out var name, out var address, out var country);
        
        return new Company
        {
            Id = id,
            Name = name,
            Address = address,
            Country = country
        };
    }
    
    public static Employee MapToEntity(this EmployeeDto employee) =>
        new ()
        {
            Id = employee.Id,
            Name = employee.Name,
            Age = employee.Age,
            Position = employee.Position
        };
}