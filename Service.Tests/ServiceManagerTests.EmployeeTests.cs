using Entities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    private IEnumerable<EmployeeDto> _employees;

    [Test]
    public async Task GetEmployees_ReturnsEmployeesList_WhenCorrectCompanyId()
    {
        var company = await _context.Companies.FirstAsync();
        var expected = _context.Employees.Where(e => 
            e.CompanyId.Equals(company.Id))
            .Select(e => e.MapToDto())
            .AsEnumerable();

        var result = await Task.Run(() => 
            _companyService.EmployeeService.GetEmployees(company.Id, trackChanges: false));
        
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
        var expected = test.MapToDto();

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
}