using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    #region Get Employees Tests
    
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

    #endregion
    
    #region Get Employee Tests
    
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
    
    #endregion

    #region Create Employee For Company Tests
    
    [Test]
    public async Task CreateEmployeeForCompany_ReturnsDtoWithCorrectId_WhenCreated()
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
    public async Task CreateEmployeeForCompany_ReturnsDtoWithCorrectName_WhenCreated()
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
    public async Task CreateEmployeeForCompany_ReturnsDtoWithCorrectAge_WhenCreated()
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
    public async Task CreateEmployeeForCompany_ReturnsDtoWithCorrectPosition_WhenCreated()
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
    public async Task CreateEmployeeForCompany_ThrowsCompanyNotFound_WhenIncorrectCompanyId()
    {
        var incorrectCompanyId = Guid.NewGuid();
        var testEmployee = new EmployeeForCreationDto("TestName", Age: 18, "TestPosition");

        Assert.Throws<CompanyNotFoundException>(() =>
            _companyService.EmployeeService.CreateEmployeeForCompany(incorrectCompanyId, testEmployee, trackChanges: false));
    }
    
    #endregion

    #region Delete Employee For Company Tests
    
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
        var testEmployee = new Employee
        {
            Id = expectedEmployeeId,
            Name = "TestName",
            Age = 0,
            Position = "TestPosition",
            CompanyId = existentCompany.Id
        };
        await _context.Employees.AddAsync(testEmployee);
        await _context.SaveChangesAsync();

        _companyService.EmployeeService.DeleteEmployeeForCompany(
            existentCompany.Id, expectedEmployeeId, trackChanges: true);
        
        Assert.That(await _context.Employees.AnyAsync(e => e.Id == testEmployee.Id), Is.False);
    }
    
    #endregion

    #region Update Employee For Company Tests
    
    [Test]
    public async Task UpdateEmployeeForCompany_ThrowsCompanyNotFoundException_WhenIncorrectCompanyId()
    {
        var testEmployee = await _context.Employees.FirstAsync();
        var incorrectCompanyId = Guid.NewGuid();
        var updateDto = new EmployeeForUpdateDto("UpdatedName", 10, "UpdatedPosition");
        
        Assert.Throws<CompanyNotFoundException>(() =>
            _companyService.EmployeeService.UpdateEmployeeForCompany(
                incorrectCompanyId, testEmployee.Id, updateDto, compTrackChanges: false, empTrackChanges: false));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_ThrowsEmployeeNotFoundException_WhenIncorrectCompanyId()
    {
        var testCompany = await _context.Companies.FirstAsync();
        var incorrectEmployeeId = Guid.NewGuid();
        var updateDto = new EmployeeForUpdateDto("UpdatedName", 10, "UpdatedPosition");
        
        Assert.Throws<EmployeeNotFoundException>(() =>
            _companyService.EmployeeService.UpdateEmployeeForCompany(
                testCompany.Id, incorrectEmployeeId, updateDto, compTrackChanges: false, empTrackChanges: false));
    }

    [Test]
    public async Task UpdateEmployeeForCompany_UpdatesName_WhenOnlyNameForChangePassed()
    {
        var expected = await _context.Employees.FirstAsync();
        var updateDto = new EmployeeForUpdateDto("UpdatedName", Age:-1, Position:null);
        expected.Name = updateDto.Name;

        _companyService.EmployeeService.UpdateEmployeeForCompany(
            expected.CompanyId, expected.Id, updateDto, compTrackChanges: false, empTrackChanges: true);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_UpdatesAge_WhenOnlyAgeForChangePassed()
    {
        var expected = await _context.Employees.FirstAsync();
        var updateDto = new EmployeeForUpdateDto(Name:null, Age: 100, Position:null);
        expected.Age = updateDto.Age;

        _companyService.EmployeeService.UpdateEmployeeForCompany(
            expected.CompanyId, expected.Id, updateDto, compTrackChanges: false, empTrackChanges: true);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_UpdatesPosition_WhenOnlyPositionForChangePassed()
    {
        var expected = await _context.Employees.FirstAsync();
        var updateDto = new EmployeeForUpdateDto(Name:null, Age: -1, "UpdatedPosition");
        expected.Age = updateDto.Age;

        _companyService.EmployeeService.UpdateEmployeeForCompany(
            expected.CompanyId, expected.Id, updateDto, compTrackChanges: false, empTrackChanges: true);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_Nulls()
    {
        var expected = await _context.Employees.FirstAsync();
        var updateDto = new EmployeeForUpdateDto(Name:null, Age: -1, null);

        _companyService.EmployeeService.UpdateEmployeeForCompany(
            expected.CompanyId, expected.Id, updateDto, compTrackChanges: false, empTrackChanges: true);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_EmptyString()
    {
        var expected = await _context.Employees.FirstAsync();
        var updateDto = new EmployeeForUpdateDto(Name:string.Empty, Age: -1, string.Empty);

        _companyService.EmployeeService.UpdateEmployeeForCompany(
            expected.CompanyId, expected.Id, updateDto, compTrackChanges: false, empTrackChanges: true);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_Whitespace()
    {
        var expected = await _context.Employees.FirstAsync();
        var updateDto = new EmployeeForUpdateDto(Name:" ", Age: -1, Position:" ");

        _companyService.EmployeeService.UpdateEmployeeForCompany(
            expected.CompanyId, expected.Id, updateDto, compTrackChanges: false, empTrackChanges: true);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    #endregion

    #region Get Employee For Patch Tests

    [Test]
    public async Task GetEmployeeForPatch_ThrowsCompanyNotFoundException_WhenIncorrectCompanyId()
    {
        var testEmployee = await _context.Employees.FirstAsync();
        var incorrectCompanyId = Guid.NewGuid();
        
        Assert.Throws<CompanyNotFoundException>(() =>
            _companyService.EmployeeService.GetEmployeeForPatch(
                incorrectCompanyId, testEmployee.Id, trackCompanyChanges: false, trackEmployeeChanges: false));
    }
    
    [Test]
    public async Task GetEmployeeForPatch_ThrowsEmployeeNotFoundException_WhenIncorrectCompanyId()
    {
        var testCompany = await _context.Companies.FirstAsync();
        var incorrectEmployeeId = Guid.NewGuid();
        
        Assert.Throws<EmployeeNotFoundException>(() =>
            _companyService.EmployeeService.GetEmployeeForPatch(
                testCompany.Id, incorrectEmployeeId, trackCompanyChanges: false, trackEmployeeChanges: false));
    }

    [Test]
    public async Task GetEmployeeForPatch_ReturnsCorrectEmployeeForUpdateDto_WhenPassedCorrectArguments()
    {
        var testEmployee = await _context.Employees.FirstAsync();
        var expected = testEmployee.MapToEmployeeForUpdateDto();

        _companyService.EmployeeService.GetEmployeeForPatch(
            testEmployee.CompanyId, testEmployee.Id, trackCompanyChanges: false, trackEmployeeChanges: false);
        var entityFromDb = await _context.Employees.FirstAsync(e => e.Id.Equals(testEmployee.Id));
        var result = entityFromDb.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task GetEmployeeForPatch_ReturnsCorrectEmployee_WhenPassedCorrectArguments()
    {
        var expected = await _context.Employees.FirstAsync();

        _companyService.EmployeeService.GetEmployeeForPatch(
            expected.CompanyId, expected.Id, trackCompanyChanges: false, trackEmployeeChanges: false);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    #endregion

    #region Save Changes For Patch
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenCalled()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: "UpdatedName", 20, "UpdatedPosition");

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenCalledWithNegativeAge_AgeShouldBeZero()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: "UpdatedName", -1, "UpdatedPosition");

        expected.Name = testPatchDto.Name;
        expected.Age = 0;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenCalledWithZeroAge_AgeShouldBeZero()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: "UpdatedName", 0, "UpdatedPosition");

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenNameIsNull_NameShouldBeEmpty()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: null, 20, "UpdatedPosition");

        expected.Name = string.Empty;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenNameIsEmpty_NameShouldBeEmpty()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: string.Empty, 20, "UpdatedPosition");

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenNameIsWhitespace_NameShouldBeWhitespace()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: " ", 20, "UpdatedPosition");

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenPositionIsWhitespace_PositionShouldBeWhitespace()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: "UpdatedName", 20, " ");

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenPositionIsEmpty_PositionShouldBeEmpty()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: "UpdatedName", 20, string.Empty);

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = testPatchDto.Position;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task SaveChangesForPatch_SavesToDb_WhenPositionIsNull_PositionShouldBeEmpty()
    {
        var expected = await _context.Employees.FirstAsync();
        var testPatchDto = new EmployeeForUpdateDto(Name: "UpdatedName", 20, Position: null);

        expected.Name = testPatchDto.Name;
        expected.Age = testPatchDto.Age;
        expected.Position = string.Empty;

        _companyService.EmployeeService.SaveChangesForPatch(testPatchDto, expected);
        var result = await _context.Employees.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    #endregion
}