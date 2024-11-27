using Entities.Models;
using Shared.DTO;

namespace Shared.Extensions;

public static class MapperExtensions
{
    public static CompanyDto MapToCompanyDto(this Company company) =>
        new ()
        {
            Id = company.Id,
            Name = company.Name ?? string.Empty,
            FullAddress = string.Join(' ', company.Address, company.Country)
        };

    public static CompanyForCreationDto MapToCompanyForCreationDto(
        this Company company, params IEnumerable<EmployeeForCreationDto> optionalEmployees) =>
        new
        (
            Name: company.Name ?? string.Empty,
            Address: company.Address ?? string.Empty,
            Country: company.Country ?? string.Empty,
            Employees: optionalEmployees
        );
    
    public static EmployeeDto MapToEmployeeDto(this Employee employee) =>
        new ()
        {
            Id = employee.Id,
            Name = employee.Name ?? string.Empty,
            Age = employee.Age,
            Position = employee.Position ?? string.Empty
        };
    
    public static EmployeeForCreationDto MapToEmployeeForCreationDto(this Employee employee) =>
        new 
        (
            Name: employee.Name ?? string.Empty,
            Age: employee.Age,
            Position: employee.Position ?? string.Empty
        );
    
    public static EmployeeForUpdateDto MapToEmployeeForUpdateDto(this Employee employee) =>
        new 
        (
            Name: employee.Name ?? string.Empty,
            Age: employee.Age,
            Position: employee.Position ?? string.Empty
        );

    public static Company MapToEntity(this CompanyForCreationDto company) =>
        new ()
        {
            Name = company.Name,
            Address = company.Address,
            Country = company.Country,
            Employees = company.Employees?.Select(e => e.MapToEntity()).ToList() ?? []
        };

    public static Employee MapToEntity(this EmployeeForCreationDto employee) =>
        new()
        {
            Name = employee.Name,
            Age = employee.Age,
            Position = employee.Position
        };
}