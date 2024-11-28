using Entities.Models;

namespace Contracts.Repository;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges);
    Task<Employee?> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployeeForCompany(Employee employee);
}