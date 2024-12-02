using DataProvider;
using Repository;
using Service.DataShaping;
using Shared.DTO;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    private RepositoryContext _context;
    private ServiceManager _companyService;
    
    [SetUp]
    public async Task Setup()
    {
        var seedDataProvider = new SeedDataProvider();
        
        _context = await InMemoryDatabaseProvider.CreateDatabaseContext(
            seedDataProvider.Companies, seedDataProvider.Employees
        );

        var repoManger = new RepositoryManager(_context);
        var dataShaper = new DataShaper<EmployeeDto>();
        _companyService = new ServiceManager(repoManger, dataShaper);
    }

    [TearDown]
    public async Task Teardown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}