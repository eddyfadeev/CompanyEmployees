using Contracts.Repository;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DTO;
using Shared.Extensions;
using Shared.RequestFeatures;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;

    public EmployeeService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployees(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);

        var employeesWithMetaData = await _repository.Employee.GetEmployees(companyId, employeeParameters, trackChanges);

        return (employees: employeesWithMetaData.Select(e => e.MapToEmployeeDto()),
                metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);

        var employee = await TryGetEmployee(companyId, employeeId, trackChanges);

        return employee.MapToEmployeeDto();
    }

    public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);

        var entity = employee.MapToEntity();
        _repository.Employee.CreateEmployeeForCompany(companyId, entity);
        await _repository.SaveAsync();

        return entity.MapToEmployeeDto();
    }

    public async Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);

        var employeeForCompany = await TryGetEmployee(companyId, employeeId, trackChanges);

        _repository.Employee.DeleteEmployeeForCompany(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto? employee, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);
        
        var employeeToUpdate = await TryGetEmployee(companyId, employeeId, trackChanges);

        employeeToUpdate.UpdateEntity(employee!);
        await _repository.SaveAsync();
    }
    
    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> 
        GetEmployeeForPatch(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);

        var employeeEntity = await TryGetEmployee(companyId, employeeId, trackChanges);

        var employeeToPatch = employeeEntity.MapToEmployeeForUpdateDto();

        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        employeeEntity.PatchEntity(employeeToPatch);
        await _repository.SaveAsync();
    }

    private async Task CheckIfCompanyExists(Guid companyId)
    {
        if (!await _repository.Company.CompanyExists(companyId))
        {
            throw new CompanyNotFoundException(companyId);
        }
    }

    private async Task<Employee> TryGetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        await _repository.Employee.EmployeeExists(employeeId)
            ? await _repository.Employee.GetEmployee(companyId, employeeId, trackChanges)
            : throw new EmployeeNotFoundException(employeeId)!;
}