using Contracts;
using Contracts.Logging;
using Contracts.Repository;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DTO;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(ILoggerManager logger, IRepositoryManager repositoryManager,
        IDataShaper<EmployeeDto> dataShaper, UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
    {
        _companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager));
        
        _employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, dataShaper));
        
        _authenticationService = new Lazy<IAuthenticationService>(() => 
            new AuthenticationService(logger, userManager, configuration));
    }    
    
    public ICompanyService CompanyService => 
        _companyService.Value;
    
    public IEmployeeService EmployeeService => 
        _employeeService.Value;

    public IAuthenticationService AuthenticationService => 
        _authenticationService.Value;
}