using Contracts.Repository;
using Entities.Exceptions;
using Entities.Models;
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

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
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

    public void DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeForCompany = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employeeForCompany is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        _repository.Employee.DeleteEmployeeForCompany(employeeForCompany);
        _repository.Save();
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, 
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
        
        var employeeToUpdate = _repository.Employee.GetEmployee(companyId, employeeId, empTrackChanges);
        if (employeeToUpdate is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        employeeToUpdate.UpdateEntity(employee);
        _repository.Save();
    }
    
    public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) 
        GetEmployeeForPatch(Guid companyId, Guid employeeId, bool trackCompanyChanges, bool trackEmployeeChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackCompanyChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = _repository.Employee.GetEmployee(companyId, employeeId, trackEmployeeChanges);
        if (employeeEntity is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        var employeeToPatch = employeeEntity.MapToEmployeeForUpdateDto();

        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        employeeEntity.UpdateEntity(employeeToPatch);
        _repository.Save();
    }
}