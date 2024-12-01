using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges) =>
        await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name)
            .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            .Take(employeeParameters.PageSize)
            .ToListAsync();

    public async Task<Employee?> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        await FindByCondition(e => 
            e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges)
            .SingleOrDefaultAsync();

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployeeForCompany(Employee employee) => 
        Delete(employee);

    public async Task<bool> EmployeeExists(Guid employeeId) =>
        await RepositoryContext.Employees.AnyAsync(e => e.Id.Equals(employeeId));
}