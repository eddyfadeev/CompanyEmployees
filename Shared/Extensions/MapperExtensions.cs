using Entities.Models;
using Shared.DTO;

namespace Shared.Extensions;

public static class MapperExtensions
{
    public static CompanyDto MapToCompanyDto(this Company company) =>
        new()
        {
            Id = company.Id,
            Name = company.Name ?? string.Empty,
            FullAddress = string.Join(' ', company.Address, company.Country)
        };

    public static CompanyForCreationDto MapToCompanyForCreationDto(
        this Company company, params IEnumerable<EmployeeForCreationDto> optionalEmployees) =>
        new ()
        {
            Name = company!.Name,
            Address = company!.Address,
            Country = company.Country,
            Employees = optionalEmployees
        };

public static EmployeeDto MapToEmployeeDto(this Employee employee) =>
        new ()
        {
            Id = employee.Id,
            Name = employee.Name ?? string.Empty,
            Age = employee.Age,
            Position = employee.Position ?? string.Empty
        };
    
    public static EmployeeForCreationDto MapToEmployeeForCreationDto(this Employee employee) =>
        new ()
        {
            Name = employee.Name ?? string.Empty,
            Age = employee.Age,
            Position = employee.Position ?? string.Empty
        };
    
    public static EmployeeForUpdateDto MapToEmployeeForUpdateDto(this Employee employee) =>
        new ()
        {
            Name = employee.Name ?? string.Empty,
            Age = employee.Age,
            Position = employee.Position ?? string.Empty
        };

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

    public static User MapToEntity(this UserForRegistrationDto user) =>
        new()
        {
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

    public static Employee UpdateEntity(this Employee employee, EmployeeForUpdateDto updateDto)
    {
        employee.Name = string.IsNullOrWhiteSpace(updateDto.Name) ? employee.Name : updateDto.Name.Trim();
        employee.Age = updateDto.Age is < 199 and > 0 ? updateDto.Age : employee.Age;
        employee.Position = string.IsNullOrWhiteSpace(updateDto.Position) ? employee.Position : updateDto.Position.Trim();
        
        return employee;
    }

    public static Company UpdateEntity(this Company company, CompanyForUpdateDto updateDto)
    {
        company.Name = string.IsNullOrWhiteSpace(updateDto.Name) ? company.Name : updateDto.Name;
        company.Address = string.IsNullOrWhiteSpace(updateDto.Address) ? company.Address : updateDto.Address;
        company.Country = string.IsNullOrWhiteSpace(updateDto.Country) ? company.Country : updateDto.Country;
        company.Employees = (updateDto.Employees is null || !updateDto.Employees.Any()
            ? company.Employees
            : updateDto.Employees.Select(e => e.MapToEntity()).ToList());

        return company;
    }
    
    public static Employee PatchEntity(this Employee employee, EmployeeForUpdateDto updateDto)
    {
        employee.Name = string.IsNullOrEmpty(updateDto.Name) ? string.Empty : updateDto.Name;
        employee.Age = updateDto.Age < 0 ? 0 : updateDto.Age;
        employee.Position = string.IsNullOrEmpty(updateDto.Position) ? string.Empty : updateDto.Position;

        return employee;
    }
}