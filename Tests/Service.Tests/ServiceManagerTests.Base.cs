using Repository;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    private RepositoryContext _context;
    private ServiceManager _companyService;
    private IEnumerable<CompanyDto> _companies;
    private IEnumerable<EmployeeDto> _employees;
    
    [SetUp]
    public async Task Setup()
    {
        var seedDataProvider = new SeedDataProvider();
        _companies = seedDataProvider.Companies.Select(c => c.MapToCompanyDto());
        _employees = seedDataProvider.Employees.Select(e => e.MapToEmployeeDto());
        
        _context = await InMemoryDatabaseProvider.CreateDatabaseContext(
            seedDataProvider.Companies, seedDataProvider.Employees
        );

        var repoManger = new RepositoryManager(_context);
        _companyService = new ServiceManager(repoManger);
    }

    [TearDown]
    public async Task Teardown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}