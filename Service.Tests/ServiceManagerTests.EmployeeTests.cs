using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    [Test]
    public async Task GetEmployees_ReturnsEmployeesList_WhenCorrectCompanyId()
    {
        var company = await _context.Companies.FirstAsync();
        var expected = _context.Employees.Where(e => 
            e.CompanyId.Equals(company.Id))
            .Select(e => e.MapToEmployeeDto())
            .AsEnumerable();

        var result = await Task.Run(() => 
            _companyService.EmployeeService.GetEmployees(company.Id, trackChanges: false));
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetEmployees_ReturnsEmptyList_WhenNoEmployees()
    {
        var testId = Guid.NewGuid();
        var companyToCreate = new Company
        {
            Id = testId,
            Name = "Empty Company",
            Address = string.Empty,
            Country = string.Empty,
        };
        await _context.Companies.AddAsync(companyToCreate);
        await _context.SaveChangesAsync();
        var expected = _context.Employees
            .Where(e => e.CompanyId == companyToCreate.Id)
            .Select(e => e.MapToEmployeeDto())
            .AsEnumerable();

        var result = _companyService.EmployeeService.GetEmployees(companyToCreate.Id, trackChanges: false);
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetEmployees_ThrowsCompanyNotFoundException_WhenIncorrectCompanyId()
    {
        var incorrectCompanyId = Guid.NewGuid();

        Assert.Throws<CompanyNotFoundException>(() =>
            _companyService.EmployeeService.GetEmployees(incorrectCompanyId, trackChanges: false));
    }

    [Test]
    public async Task GetEmployee_ReturnsCorrectEmployee()
    {
        var test = await _context.Employees.FirstAsync();
        var expected = test.MapToEmployeeDto();

        var result = _companyService.EmployeeService.GetEmployee(test.CompanyId, test.Id, trackChanges: false);
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetEmployee_ThrowsCompanyNotFoundException_WhenIncorrectCompanyIdProvided()
    {
        var test = await _context.Employees.FirstAsync();
        var incorrectCompanyId = Guid.NewGuid();

        Assert.Throws<CompanyNotFoundException>(() =>
            _companyService.EmployeeService.GetEmployee(incorrectCompanyId, test.Id, trackChanges: false));
    }

    [Test]
    public async Task GetEmployee_ThrowsEmployeeNotFoundException_WhenIncorrectEmployeeIdProvided()
    {
        var test = await _context.Companies.FirstAsync();
        var incorrectEmployeeId = Guid.NewGuid();

        Assert.Throws<EmployeeNotFoundException>(() =>
            _companyService.EmployeeService.GetEmployee(test.Id, incorrectEmployeeId, trackChanges: false));
    }

    [Test]
    public async Task CreateEmployee_ReturnsEmployeeDto_WhenCreated()
    {
        var testCompany = await _context.Companies.FirstAsync();
        var testEmployee = new EmployeeForCreationDto
        (
            Name: string.Empty,
            Age: default,
            Position: string.Empty
        );
        
        var result =
            _companyService.EmployeeService.CreateEmployeeForCompany(testCompany.Id, testEmployee, trackChanges: false);
        
        Assert.That(result, Is.TypeOf<EmployeeDto>());
    }

    [Test]
    public async Task CreateEmployee_ReturnsDtoWithCorrectId_WhenCreated()
    {
        var testId = Guid.NewGuid();
        var testCompany = new Company
        {
            Id = testId,
            Name = "Test",
            Address = string.Empty,
            Country = string.Empty,
        };
        await _context.Companies.AddAsync(testCompany);
        await _context.SaveChangesAsync();
        
        var testEmployee = new EmployeeForCreationDto
        (
            Name: string.Empty,
            Age: default,
            Position: string.Empty
        );
        
        var result = _companyService.EmployeeService.CreateEmployeeForCompany(testCompany.Id, testEmployee, trackChanges: false);

        var expected = await _context.Employees.FirstAsync(e => e.CompanyId.Equals(testCompany.Id));
        
        Assert.That(result.Id, Is.EqualTo(expected.Id));
    }

    [Test]
    public async Task CreateEmployee_ReturnsDtoWithCorrectName_WhenCreated()
    {
        const string expectedName = "TestName";
        var testId = Guid.NewGuid();
        var testCompany = new Company
        {
            Id = testId,
            Name = "Test",
            Address = string.Empty,
            Country = string.Empty,
        };
        await _context.Companies.AddAsync(testCompany);
        await _context.SaveChangesAsync();
        
        var testEmployee = new EmployeeForCreationDto
        (
            Name: expectedName,
            Age: default,
            Position: string.Empty
        );
        
        var result = _companyService.EmployeeService.CreateEmployeeForCompany(testCompany.Id, testEmployee, trackChanges: false);
        
        Assert.That(result.Name, Is.EqualTo(expectedName));
    }

    [Test]
    public async Task CreateEmployee_ReturnsDtoWithCorrectAge_WhenCreated()
    {
        const int expectedAge = 32;
        var testId = Guid.NewGuid();
        var testCompany = new Company
        {
            Id = testId,
            Name = "Test",
            Address = string.Empty,
            Country = string.Empty,
        };
        await _context.Companies.AddAsync(testCompany);
        await _context.SaveChangesAsync();
        
        var testEmployee = new EmployeeForCreationDto
        (
            Name: string.Empty,
            Age: expectedAge,
            Position: string.Empty
        );
        
        var result = _companyService.EmployeeService.CreateEmployeeForCompany(testCompany.Id, testEmployee, trackChanges: false);
        
        Assert.That(result.Age, Is.EqualTo(expectedAge));
    }

    [Test]
    public async Task CreateEmployee_ReturnsDtoWithCorrectPosition_WhenCreated()
    {
        const string expectedPosition = "TestPosition";
        var testId = Guid.NewGuid();
        var testCompany = new Company
        {
            Id = testId,
            Name = "Test",
            Address = string.Empty,
            Country = string.Empty,
        };
        await _context.Companies.AddAsync(testCompany);
        await _context.SaveChangesAsync();
        
        var testEmployee = new EmployeeForCreationDto
        (
            Name: string.Empty,
            Age: default,
            Position: expectedPosition
        );
        
        var result = _companyService.EmployeeService.CreateEmployeeForCompany(testCompany.Id, testEmployee, trackChanges: false);
        
        Assert.That(result.Position, Is.EqualTo(expectedPosition));
    }

    [Test]
    public async Task CreateEmployee_ThrowsCompanyNotFound_WhenIncorrectCompanyId()
    {
        var incorrectCompanyId = Guid.NewGuid();
        var testEmployee = new EmployeeForCreationDto("TestName", Age: 18, "TestPosition");

        Assert.Throws<CompanyNotFoundException>(() =>
            _companyService.EmployeeService.CreateEmployeeForCompany(incorrectCompanyId, testEmployee, trackChanges: false));
    }

    [Test]
    public async Task DeleteEmployeeForCompany_ThrowsCompanyNotFoundException_WhenIncorrectCompanyIdPassed()
    {
        var incorrectCompanyId = Guid.NewGuid();
        var existentEmployee = await _context.Employees.FirstAsync();

        Assert.Throws<CompanyNotFoundException>(() => 
            _companyService.EmployeeService.DeleteEmployeeForCompany(incorrectCompanyId, existentEmployee.Id, trackChanges: false));
    }
    
    [Test]
    public async Task DeleteEmployeeForCompany_ThrowsCompanyNotFoundException_WhenIncorrectCompanyAndIncorrectEmployeeIdsPassed()
    {
        var incorrectCompanyId = Guid.NewGuid();
        var incorrectEmployeeId = Guid.NewGuid();

        Assert.Throws<CompanyNotFoundException>(() => 
            _companyService.EmployeeService.DeleteEmployeeForCompany(incorrectCompanyId, incorrectEmployeeId, trackChanges: false));
    }
    
    [Test]
    public async Task DeleteEmployeeForCompany_ThrowsEmployeeNotFoundException_WhenIncorrectEmployeeIdPassed()
    {
        var company = await _context.Companies.FirstAsync();
        var incorrectEmployeeId = Guid.NewGuid();

        Assert.Throws<EmployeeNotFoundException>(() => 
            _companyService.EmployeeService.DeleteEmployeeForCompany(company.Id, incorrectEmployeeId, trackChanges: false));
    }

    [Test]
    public async Task DeleteEmployeeForCompany_CorrectlyDeletes_WhenCorrectParametersPassed()
    {
        var existentCompany = await _context.Companies.FirstAsync();
        var expectedEmployeeId = Guid.NewGuid();
        var testEmployee = new Employee()
        {
            Id = expectedEmployeeId,
            Name = "TestName",
            Age = 0,
            Position = "TestPosition",
            CompanyId = existentCompany.Id
        };
        await _context.Employees.AddAsync(testEmployee);
        await _context.SaveChangesAsync();
        _context.Entry(testEmployee).State = EntityState.Detached;

        _companyService.EmployeeService.DeleteEmployeeForCompany(
            existentCompany.Id, expectedEmployeeId, trackChanges: false);
        
        Assert.That(await _context.Employees.AnyAsync(e => e.Id == testEmployee.Id), Is.False);
    }
}