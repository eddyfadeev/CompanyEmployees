using Contracts.Logging;
using DataProvider;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Repository;
using Service.DataShaping;
using Shared.DTO;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    private RepositoryContext _context;
    private ServiceManager _companyService;
    
    
    /*
     *IUserStore<TUser> store, 
    IOptions<IdentityOptions> optionsAccessor, 
    IPasswordHasher<TUser> passwordHasher, 
    IEnumerable<IUserValidator<TUser>> userValidators, 
    IEnumerable<IPasswordValidator<TUser>> passwordValidators, 
    ILookupNormalizer keyNormalizer, 
    IdentityErrorDescriber errors, 
    IServiceProvider services, 
    ILogger<UserManager<TUser>> logge
     * 
     */
    [SetUp]
    public async Task Setup()
    {
        var userManager = new UserManager<User>(
            Mock.Of<IUserStore<User>>(),
            Mock.Of<IOptions<IdentityOptions>>(), 
            Mock.Of<IPasswordHasher<User>>(),
            new List<IUserValidator<User>> { Mock.Of<IUserValidator<User>>() },
            new List<IPasswordValidator<User>> { Mock.Of<IPasswordValidator<User>>() },
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(), 
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<User>>>()
            );
        
        var seedDataProvider = new SeedDataProvider();
        
        _context = await InMemoryDatabaseProvider.CreateDatabaseContext(
            seedDataProvider.Companies, seedDataProvider.Employees
        );

        var repoManger = new RepositoryManager(_context);
        _companyService = new ServiceManager(Mock.Of<ILoggerManager>(), repoManger, Mock.Of<DataShaper<EmployeeDto>>(), userManager, Mock.Of<IOptions<JwtConfiguration>>());
    }

    [TearDown]
    public async Task Teardown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}