namespace Shared.DTO;

public record CompanyForCreationDto(string Name, string Address, string Country, params IEnumerable<EmployeeForCreationDto> Employees);