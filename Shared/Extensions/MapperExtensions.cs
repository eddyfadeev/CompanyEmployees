using Entities.Models;
using Shared.DTO;

namespace Shared.Extensions;

public static class MapperExtensions
{
    public static CompanyDto MapToDto(this Company company) =>
        new ()
        {
            Id = company.Id,
            Name = company.Name ?? string.Empty,
            FullAddress = string.Join(' ', company.Address, company.Country)
        };
    
    public static EmployeeDto MapToDto(this Employee employee) =>
        new ()
        {
            Id = employee.Id,
            Name = employee.Name ?? string.Empty,
            Age = employee.Age,
            Position = employee.Position ?? string.Empty
        };

    public static Company MapToEntity(this CompanyForCreationDto company) =>
        new ()
        {
            Name = company.Name,
            Address = company.Address,
            Country = company.Country
        };

    public static Employee MapToEntity(this EmployeeForCreationDto employee) =>
        new()
        {
            Name = employee.Name,
            Age = employee.Age,
            Position = employee.Position
        };
}