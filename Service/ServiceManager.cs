using Contracts;
using Contracts.Repository;
using Service.Contracts;
using Shared.DTO;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(IRepositoryManager repositoryManager, IDataShaper<EmployeeDto> dataShaper)
    {
        _companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager));
        
        _employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, dataShaper));
    }    
    
    public ICompanyService CompanyService => 
        _companyService.Value;
    
    public IEmployeeService EmployeeService => 
        _employeeService.Value;
}