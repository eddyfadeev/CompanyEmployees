using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts.Repository;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters ,bool trackChanges);
    Task<Employee?> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployeeForCompany(Employee employee);
    Task<bool> EmployeeExists(Guid employeeId);
}