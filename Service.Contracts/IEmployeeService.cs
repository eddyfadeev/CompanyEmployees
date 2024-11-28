using Entities.Models;
using Shared.DTO;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges);
    Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges);
    Task UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, 
        bool compTrackChanges, bool empTrackChanges);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> 
        GetEmployeeForPatch(Guid companyId, Guid employeeId, bool trackCompanyChanges, bool trackEmployeeChanges);
    Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
}