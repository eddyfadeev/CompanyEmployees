using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    [Test]
    public async Task GetAllCompanies_ReturnsAllCompanies_WhenDbEntriesExist()
    {
        var expected = _companies.ToList();

        var result = await Task.Run(() => 
            _companyService.CompanyService.GetAllCompanies(trackChanges: false));
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetAllCompanies_ReturnsEmptyList_WhenNoEntriesInDb()
    {
        var entities = await _context.Companies.ToListAsync();
        _context.RemoveRange(entities);
        await _context.SaveChangesAsync();
        
        var result = await Task.Run(() => 
            _companyService.CompanyService.GetAllCompanies(trackChanges: false));
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetCompany_ReturnsCorrectCompany()
    {
        var expected = _companies.FirstOrDefault();
        Assert.That(expected, Is.Not.Null, "Error getting test company from Db");

        var result = await Task.Run(() => 
            _companyService.CompanyService.GetCompany(expected.Id, trackChanges: false));
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetCompany_ThrowsCompanyNotFoundException_WhenWrongId()
    {
        var wrongId = Guid.NewGuid();
        Assert.Throws<CompanyNotFoundException>(() => 
            _companyService.CompanyService.GetCompany(wrongId, trackChanges: false));
    }
    
    
    [Test]
    public async Task CreateCompany_ReturnsCompanyDto_WhenCompanyCreated()
    {
        var dtoToCreate = new CompanyForCreationDto("Name", "Address", "Country");
        
        var result = _companyService.CompanyService.CreateCompany(dtoToCreate);
        
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task CreateCompany_ReturnsDtoWithCorrectName_WhenCompanyCreated()
    {
        var testCompany = new Company
        {
            Name = "Create Test"
        };
        var expectedDto = testCompany.MapToCompanyDto();
        var dtoToCreate = new CompanyForCreationDto(testCompany.Name, Address: string.Empty, Country: string.Empty);
        
        var result = _companyService.CompanyService.CreateCompany(dtoToCreate);
        
        Assert.That(result.Name, Is.EqualTo(expectedDto.Name));
    }
    
    [Test]
    public async Task CreateCompany_ReturnsDtoWithCorrectAddress_WhenCompanyCreated()
    {
        var testCompany = new Company
        {
            Address = "123 Test Street, T6W 1T7, Ab",
            Country = "Canada"
        };
        var expectedDto = testCompany.MapToCompanyDto();
        var dtoToCreate = new CompanyForCreationDto(Name: string.Empty, testCompany.Address, testCompany.Country);
        
        var result = _companyService.CompanyService.CreateCompany(dtoToCreate);
        
        Assert.That(result.FullAddress, Is.EqualTo(expectedDto.FullAddress));
    }
    
    [Test]
    public async Task CreateCompany_ReturnsDtoWithCorrectId_WhenCompanyCreated()
    {
        var testCompany = new CompanyForCreationDto("Test", string.Empty, string.Empty);

        var result = _companyService.CompanyService.CreateCompany(testCompany);
        
        var expected = await _context.Companies.FirstAsync(c => c.Name == "Test");

        Assert.That(result.Id, Is.EqualTo(expected.Id));
    }
    
    [Test]
    public async Task CreateCompany_AddsEmployees_WhenCompanyCreatedWithEmployees()
    {
        var testEmployees = await _context.Employees.ToListAsync();
        // use MapToEmployeeForCreationDto extension to cut off the id
        var expectedEmployees = testEmployees.Select(e => 
            e.MapToEmployeeForCreationDto())
            .ToList();
        var testCompany = new CompanyForCreationDto(
            Name: "TestName", 
            Address: string.Empty, 
            Country: string.Empty, 
            Employees: testEmployees.Select(e => e.MapToEmployeeForCreationDto()));

        await _context.Database.EnsureDeletedAsync();
        _companyService.CompanyService.CreateCompany(testCompany);
        // use MapToEmployeeForCreationDto extension to cut off the id
        var result = await _context.Employees.Select(e => 
            e.MapToEmployeeForCreationDto())
            .ToListAsync();

        Assert.That(result, Is.EquivalentTo(expectedEmployees));
    }
    
    [Test]
    public async Task CreateCompany_DoesNotAddEmployees_WhenCompanyCreatedWithEmptyListOfEmployees()
    {
        List<EmployeeForCreationDto> emptyEmployeesList = new ();
        
        var testCompany = new CompanyForCreationDto(
            Name: "TestName", 
            Address: string.Empty, 
            Country: string.Empty, 
            Employees: emptyEmployeesList);

        await _context.Database.EnsureDeletedAsync();
        _companyService.CompanyService.CreateCompany(testCompany);
        
        var result = await _context.Employees.ToListAsync();

        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public async Task GetByIds_ReturnsListWithCompanyDtos()
    {
        var testIds = _context.Companies.Select(c => c.Id).AsEnumerable();

        var result = _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result.First(), Is.TypeOf<CompanyDto>());
        
    }

    [Test]
    public async Task GetByIds_ReturnsNotEmptyListOfCompanies_WhenIdsArePresentInDatabase()
    {
        var testIds = _context.Companies.Select(c => c.Id).AsEnumerable();

        var result = _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result, Is.Not.Empty);
        
    }

    [Test]
    public async Task GetByIds_ReturnsEmptyList_WhenPassedEmptyIdsList()
    {
        List<Guid> testIds = [];

        var result = _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByIds_ReturnsCorrectCompanies()
    {
        var expectedCompanies = _context.Companies.Select(c => 
            c.MapToCompanyDto()).AsEnumerable();
        var testIds = expectedCompanies.Select(c => c.Id).ToList();
        
        var result = _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result, Is.EqualTo(expectedCompanies));
    }

    [Test]
    public async Task GetByIds_ThrowsCollectionByIdsBadRequestException_WhenIdsAndCompaniesCountMismatch()
    {
        List<Guid> testIds = [Guid.NewGuid(), Guid.NewGuid()];

        Assert.Throws<CollectionByIdsBadRequestException>(() =>
        {
            _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        });
    }
    
    [Test]
    public async Task GetByIds_ThrowsIdParametersBadRequestException_WhenIdsAreNull()
    {
        Assert.Throws<IdParametersBadRequestException>(() =>
        {
            _companyService.CompanyService.GetByIds(null, trackChanges: false);
        });
    }
}