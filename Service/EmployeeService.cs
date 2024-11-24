using Contracts.Logging;
using Contracts.Repository;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DTO;
using Shared.Extensions;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
        
        return employees.Select(e => e.MapToDto());
    }
}