using DataProvider;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Tests;

public class CompanyRepositoryTests
{
    private RepositoryContext _context;
    private CompanyRepository _repository;

    [SetUp]
    public async Task Setup()
    {
        var seedDataProvider = new SeedDataProvider();
        
        _context = await InMemoryDatabaseProvider.CreateDatabaseContext(
            seedDataProvider.Companies, seedDataProvider.Employees
        );

        _repository = new CompanyRepository(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
    }

    [Test]
    public async Task GetAllCompanies_ReturnsOrderedCompanies()
    {
        var result = await _repository.GetAllCompanies(trackChanges: false);
        
        Assert.That(result, Is.Ordered.By(nameof(Company.Name)));
    }
    
    [Test]
    public async Task GetAllCompanies_ReturnsCorrectCompanies()
    {
        var expected = await _context.Companies.ToListAsync();
        
        var result = await _repository.GetAllCompanies(trackChanges: false);
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetCompany_ExistingId_ReturnsInstanceOfCompany()
    {
        var expected = await _context.Companies.FirstAsync();
        
        var result = await _repository.GetCompany(expected.Id, trackChanges: false);

        Assert.That(result, Is.InstanceOf<Company>());
    }
    
    [Test]
    public async Task GetCompany_ExistingId_ReturnsCorrectCompany()
    {
        var expected = await _context.Companies.FirstAsync();
        
        var result = await _repository.GetCompany(expected.Id, trackChanges: false);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetCompany_NonExistingId_ReturnsNull()
    {
        var incorrectId = Guid.NewGuid();
        
        var result = await _repository.GetCompany(incorrectId, trackChanges: false);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateCompany_AddsCompanyToDatabase()
    {
        var expected = new Company 
        { 
            Id = Guid.NewGuid(), 
            Name = "NewCorp",
            Address = "TestAddress",
        };

        _repository.CreateCompany(expected);
        await _context.SaveChangesAsync();

        var result = await _context.Companies.FindAsync(expected.Id);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetByIds_ReturnsMatchingCompanies()
    {
        var expected = await _context.Companies.ToListAsync();
        var ids = expected.Select(c => c.Id);

        var result = await _repository.GetByIds(ids, trackChanges: false);

        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task GetByIds_ReturnsEmptyList_WhenNoMatchingIdsPassed()
    {
        List<Guid> ids = [Guid.Empty, Guid.NewGuid()];

        var result = await _repository.GetByIds(ids, trackChanges: false);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task DeleteCompany_RemovesCompanyFromDatabase()
    {
        var companyToDelete = await _context.Companies.FirstAsync();

        _repository.DeleteCompany(companyToDelete);
        await _context.SaveChangesAsync();

        var deletedCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id.Equals(companyToDelete.Id));
        
        Assert.That(deletedCompany, Is.Null);
    }
    
    [Test]
    public async Task UpdateCompany_UpdatesInDatabase()
    {
        var expected = await _context.Companies.FirstAsync();
        expected.Name = "UpdatedName";

        _repository.Update(expected);
        await _context.SaveChangesAsync();

        var result = await _context.Companies.FirstAsync(c => c.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
}