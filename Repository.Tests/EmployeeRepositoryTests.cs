using DataProvider;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository.Tests;

public class EmployeeRepositoryTests
{
    private RepositoryContext _context;
    private EmployeeRepository _repository;

    [SetUp]
    public async Task Setup()
    {
        var seedDataProvider = new SeedDataProvider();
        
        _context = await InMemoryDatabaseProvider.CreateDatabaseContext(
            seedDataProvider.Companies, seedDataProvider.Employees
        );

        _repository = new EmployeeRepository(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
    }

    [Test]
    public async Task GetEmployees_ReturnsOrderedEmployees()
    {
        var parameters = new EmployeeParameters();
        var testCompany = await _context.Companies.FirstAsync();
        
        var result = await _repository.GetEmployees(testCompany.Id, parameters, trackChanges: false);
        
        Assert.That(result, Is.Ordered.By(nameof(Company.Name)));
    }
    
    [Test]
    public async Task GetEmployees_ReturnsCorrectEmployees()
    {
        var parameters = new EmployeeParameters();
        var testCompany = await _context.Companies.FirstAsync();
        var expected = await _context.Employees.Where(e => 
                e.CompanyId.Equals(testCompany.Id))
            .ToListAsync();
        
        var result = await _repository.GetEmployees(testCompany.Id, parameters, trackChanges: false);
        
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task GetEmployees_ReturnsCorrectEmployeesAmount()
    {
        var expected = new EmployeeParameters { PageSize = 2 };
        var testCompany = await _context.Companies.FirstAsync();
        
        var result = await _repository.GetEmployees(testCompany.Id, employeeParameters: expected, trackChanges: false);
        
        Assert.That(result.Count(), Is.EqualTo(expected.PageSize));
    }
    
    [Test]
    public async Task GetEmployees_ReturnsEmptyEmployeesList_WhenNoCompanyInDb()
    {
        var parameters = new EmployeeParameters();
        var testCompanyId = Guid.NewGuid();
        
        var result = await _repository.GetEmployees(testCompanyId, parameters, trackChanges: false);
        
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public async Task GetEmployee_ReturnsCorrectEmployee_WhenCorrectIds()
    {
        var expected = await _context.Employees.FirstAsync();
        
        var result = await _repository.GetEmployee(expected.CompanyId, expected.Id, trackChanges: false);
    
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task GetEmployee_ReturnsNullWhenIncorrectCompanyId()
    {
        var testEmployee = await _context.Employees.FirstAsync();
        var incorrectCompanyId = Guid.NewGuid();
        
        var result = await _repository.GetEmployee(incorrectCompanyId, testEmployee.Id, trackChanges: false);
    
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetEmployee_ReturnsNullWhenIncorrectEmployeeId()
    {
        var testEmployee = await _context.Employees.FirstAsync();
        var incorrectEmployeeId = Guid.NewGuid();
        
        var result = await _repository.GetEmployee(testEmployee.CompanyId, incorrectEmployeeId, trackChanges: false);
    
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateEmployeeForCompany_CreatesEmployee()
    {
        var testCompany = await _context.Companies.FirstAsync();
        var testEmployee = new Employee
        {
            Name = "CreateTest",
            Age = 30,
            Position = "TestPosition",
            CompanyId = testCompany.Id
        };

        _repository.CreateEmployeeForCompany(testEmployee.CompanyId, testEmployee);
        await _context.SaveChangesAsync();
        
        var result = await _context.Employees.AnyAsync(e => 
            e.CompanyId.Equals(testEmployee.CompanyId) && e.Name.Equals(testEmployee.Name));

        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task CreateEmployeeForCompany_CreatesEmployee_WhenIncorrectCompanyId()
    {
        var testCompanyId = Guid.NewGuid();
        var testEmployee = new Employee
        {
            Name = "CreateTest",
            Age = 30,
            Position = "TestPosition",
            CompanyId = testCompanyId
        };

        _repository.CreateEmployeeForCompany(testEmployee.CompanyId, testEmployee);
        await _context.SaveChangesAsync();
        
        var result = await _context.Employees.AnyAsync(e => 
            e.CompanyId.Equals(testEmployee.CompanyId) && e.Name.Equals(testEmployee.Name));

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteEmployeeForCompany_DeletesEmployee()
    {
        var testEmployee = await _context.Employees.FirstAsync();

        _repository.DeleteEmployeeForCompany(testEmployee);
        await _context.SaveChangesAsync();
        
        var result = await _context.Employees.AnyAsync(e => e.Equals(testEmployee));
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteEmployeeForCompany_DeletesEmployee_WhenCompanyIdIsNotCorrect()
    {
        var testEmployee = await _context.Employees.FirstAsync();
        testEmployee.CompanyId = Guid.NewGuid();

        _repository.DeleteEmployeeForCompany(testEmployee);
        await _context.SaveChangesAsync();
        
        var result = await _context.Employees.AnyAsync(e => e.Id.Equals(testEmployee.Id));
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task EmployeeExists_ReturnsTrue_WhenExistsInDb()
    {
        var testEmployee = await _context.Employees.FirstAsync();

        var result = await _repository.EmployeeExists(testEmployee.Id);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task EmployeeExists_ReturnsFalse_WhenDoesNotExistsInDb()
    {
        var wrongId = Guid.NewGuid();

        var result = await _repository.EmployeeExists(wrongId);
        
        Assert.That(result, Is.False);
    }
}