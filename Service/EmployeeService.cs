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

    public async Task<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = await _repository.Employee.GetEmployees(companyId, trackChanges);
        
        return employees.Select(e => e.MapToEmployeeDto());
    }

    public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employee = await _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employee is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        return employee.MapToEmployeeDto();
    }

    public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var entity = employee.MapToEntity();
        _repository.Employee.CreateEmployeeForCompany(companyId, entity);
        await _repository.SaveAsync();

        return entity.MapToEmployeeDto();
    }

    public async Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeForCompany = await _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
        if (employeeForCompany is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        _repository.Employee.DeleteEmployeeForCompany(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, 
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, compTrackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
        
        var employeeToUpdate = await _repository.Employee.GetEmployee(companyId, employeeId, empTrackChanges);
        if (employeeToUpdate is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        employeeToUpdate.UpdateEntity(employee);
        await _repository.SaveAsync();
    }
    
    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> 
        GetEmployeeForPatch(Guid companyId, Guid employeeId, bool trackCompanyChanges, bool trackEmployeeChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackCompanyChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = await _repository.Employee.GetEmployee(companyId, employeeId, trackEmployeeChanges);
        if (employeeEntity is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        var employeeToPatch = employeeEntity.MapToEmployeeForUpdateDto();

        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        employeeEntity.PatchEntity(employeeToPatch);
        await _repository.SaveAsync();
    }
}