using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    private IEnumerable<CompanyDto> _companies;
    
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
    public async Task CreateCompany_ReturnsNotNull_WhenCompanyCreated()
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
        var expectedDto = testCompany.MapToDto();
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
        var expectedDto = testCompany.MapToDto();
        var dtoToCreate = new CompanyForCreationDto(Name: string.Empty, testCompany.Address, testCompany.Country);
        
        var result = _companyService.CompanyService.CreateCompany(dtoToCreate);
        
        Assert.That(result.FullAddress, Is.EqualTo(expectedDto.FullAddress));
    }
    
    
}