using Entities.Exceptions;
using Shared.DTO;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    private IEnumerable<CompanyDto> _companies;
    
    [Test]
    public async Task GetAllCompanies_ReturnsAllCompanies()
    {
        var expected = _companies.ToList();

        var result = 
            await Task.Run(() => _companyService.CompanyService.GetAllCompanies(trackChanges: false));
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetCompany_ReturnsCorrectCompany()
    {
        var expected = _companies.FirstOrDefault();
        Assert.That(expected, Is.Not.Null);

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
}