using Contracts.Repository;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DTO;
using Shared.Extensions;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;

    public EmployeeService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
        
        return employees.Select(e => e.MapToEmployeeDto());
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        return employee.MapToEmployeeDto();
    }

    public EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var entity = employee.MapToEntity();
        _repository.Employee.CreateEmployeeForCompany(companyId, entity);
        _repository.Save();

        return entity.MapToEmployeeDto();
    }
}