﻿using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Extensions;

namespace Service.Tests;

public partial class ServiceManagerTests
{
    #region Get All Companies Tests
    
    [Test]
    public async Task GetAllCompanies_ReturnsAllCompanies_WhenDbEntriesExist()
    {
        var expected = await _context.Companies.Select(c => c.MapToCompanyDto()).ToListAsync();

        var result = await _companyService.CompanyService.GetAllCompanies(trackChanges: false);
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetAllCompanies_ReturnsEmptyList_WhenNoEntriesInDb()
    {
        var entities = await _context.Companies.ToListAsync();
        _context.RemoveRange(entities);
        await _context.SaveChangesAsync();

        var result = await _companyService.CompanyService.GetAllCompanies(trackChanges: false);
        
        Assert.That(result, Is.Empty);
    }
    
    #endregion

    #region Get Company Tests
    
    [Test]
    public async Task GetCompany_ReturnsCorrectCompany()
    {
        var entityFromDb = await _context.Companies.FirstAsync();
        var expected = entityFromDb.MapToCompanyDto();

        var result = await _companyService.CompanyService.GetCompany(entityFromDb.Id, trackChanges: false);
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetCompany_ThrowsCompanyNotFoundException_WhenWrongId()
    {
        var wrongId = Guid.NewGuid();
        Assert.ThrowsAsync<CompanyNotFoundException>(async () => 
            await _companyService.CompanyService.GetCompany(wrongId, trackChanges: false));
    }
    
    #endregion
    
    #region Create Company Tests
    
    [Test]
    public async Task CreateCompany_ReturnsCompanyDto_WhenCompanyCreated()
    {
        var dtoToCreate = new CompanyForCreationDto
        {
            Name = "Name", 
            Address = "Address", 
            Country = "Country"
        };
        
        var result = await _companyService.CompanyService.CreateCompany(dtoToCreate);
        
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
        var dtoToCreate = new CompanyForCreationDto
        {
            Name = testCompany.Name, 
            Address = string.Empty, 
            Country = string.Empty
        };
        
        var result = await _companyService.CompanyService.CreateCompany(dtoToCreate);
        
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
        var dtoToCreate = new CompanyForCreationDto
        {
            Name = string.Empty,
            Address = testCompany.Address,
            Country = testCompany.Country
        };
        
        var result = await _companyService.CompanyService.CreateCompany(dtoToCreate);
        
        Assert.That(result.FullAddress, Is.EqualTo(expectedDto.FullAddress));
    }
    
    [Test]
    public async Task CreateCompany_ReturnsDtoWithCorrectId_WhenCompanyCreated()
    {
        var testCompany = new CompanyForCreationDto
        {
            Name = "Test", 
            Address = string.Empty, 
            Country = string.Empty
        };

        var result = await _companyService.CompanyService.CreateCompany(testCompany);
        
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
        var testCompany = new CompanyForCreationDto
        {
            Name = "TestName",
            Address = string.Empty,
            Country = string.Empty,
            Employees = testEmployees.Select(e => e.MapToEmployeeForCreationDto())
        };

        await _context.Database.EnsureDeletedAsync();
        await _companyService.CompanyService.CreateCompany(testCompany);
        // use MapToEmployeeForCreationDto extension to cut off the id
        var result = await _context.Employees.Select(e => 
            e.MapToEmployeeForCreationDto())
            .ToListAsync();

        Assert.That(result, Is.EquivalentTo(expectedEmployees));
    }
    
    [Test]
    public async Task CreateCompany_DoesNotAddEmployees_WhenCompanyCreatedWithEmptyListOfEmployees()
    {
        List<EmployeeForCreationDto> emptyEmployeesList = [];
        
        var testCompany = new CompanyForCreationDto
        {
            Name = "TestName",
            Address = string.Empty,
            Country = string.Empty,
            Employees = emptyEmployeesList
        };

        await _context.Database.EnsureDeletedAsync();
        await _companyService.CompanyService.CreateCompany(testCompany);
        
        var result = await _context.Employees.ToListAsync();

        Assert.That(result, Is.Empty);
    }
    
    #endregion
    
    #region Get By Ids Tests
    
    [Test]
    public async Task GetByIds_ReturnsListWithCompanyDtos()
    {
        var testIds = await _context.Companies.Select(c => c.Id).ToListAsync();

        var result = await _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result.First(), Is.TypeOf<CompanyDto>());
    }

    [Test]
    public async Task GetByIds_ReturnsNotEmptyListOfCompanies_WhenIdsArePresentInDatabase()
    {
        var testIds = await _context.Companies.Select(c => c.Id).ToListAsync();

        var result = await _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public async Task GetByIds_ReturnsEmptyList_WhenPassedEmptyIdsList()
    {
        List<Guid> testIds = [];

        var result = await _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByIds_ReturnsCorrectCompanies()
    {
        var expectedCompanies = await _context.Companies.Select(c => c.MapToCompanyDto()).ToListAsync();
        var testIds = expectedCompanies.Select(c => c.Id).ToList();
        
        var result = await _companyService.CompanyService.GetByIds(testIds, trackChanges: false);
        
        Assert.That(result, Is.EqualTo(expectedCompanies));
    }

    [Test]
    public async Task GetByIds_ThrowsCollectionByIdsBadRequestException_WhenIdsAndCompaniesCountMismatch()
    {
        List<Guid> testIds = [Guid.NewGuid(), Guid.NewGuid()];

        Assert.ThrowsAsync<CollectionByIdsBadRequestException>(async () =>
            await _companyService.CompanyService.GetByIds(testIds, trackChanges: false));
    }
    
    [Test]
    public async Task GetByIds_ThrowsIdParametersBadRequestException_WhenIdsAreNull()
    {
        Assert.ThrowsAsync<IdParametersBadRequestException>(async () =>
            await _companyService.CompanyService.GetByIds(null, trackChanges: false));
    }
    
    #endregion

    #region Create Company Collection Tests
    
    [Test]
    public async Task CreateCompanyCollection_ThrowsCompanyCollectionBadRequestException_WhenPassedCollectionIsNull()
    {
        Assert.ThrowsAsync<CompanyCollectionBadRequestException>(async () =>
            await _companyService.CompanyService.CreateCompanyCollection(null));
    }

    [Test]
    public async Task CreateCompanyCollection_ReturnsCompanyCollection_WhenPassedCollectionWithEntries()
    {
        var testCompanies = await _context.Companies.ToListAsync();
        var expectedCompanies = testCompanies.Select(c => c.MapToCompanyForCreationDto());
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.companies, Is.Not.Null);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsNonEmptyCollection_WhenPassedCollectionWithEntries()
    {
        var testCompanies = await _context.Companies.ToListAsync();
        var expectedCompanies = testCompanies.Select(c => c.MapToCompanyForCreationDto());
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.companies, Is.Not.Empty);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsIdsString_WhenPassedCollectionWithEntries()
    {
        var testCompanies = await _context.Companies.ToListAsync();
        var expectedCompanies = testCompanies.Select(c => c.MapToCompanyForCreationDto());
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.ids, Is.Not.Null);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsNonEmptyIdsString_WhenPassedCollectionWithEntries()
    {
        var testCompanies = await _context.Companies.ToListAsync();
        var expectedCompanies = testCompanies.Select(c => c.MapToCompanyForCreationDto());
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.ids, Is.Not.WhiteSpace);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsEmptyCollection_WhenPassedEmptyCollection()
    {
        var expectedCompanies = new List<CompanyForCreationDto>();
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.companies, Is.Empty);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCollection_NullCheck_WhenPassedEmptyCollection()
    {
        var expectedCompanies = new List<CompanyForCreationDto>();
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.companies, Is.Not.Null);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsNotNullIdsString_WhenPassedEmptyCollection()
    {
        var expectedCompanies = new List<CompanyForCreationDto>();
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.ids, Is.Not.Null);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsEmptyIdsString_WhenPassedEmptyCollection()
    {
        var expectedCompanies = new List<CompanyForCreationDto>();
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(expectedCompanies);
        
        Assert.That(result.ids, Is.Empty);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCorrectCollection_WhenPassedCollectionWithEntries()
    {
        var testCompanies = await _context.Companies.ToListAsync();
        var companiesForInserting = testCompanies.Select(c => c.MapToCompanyForCreationDto());
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(companiesForInserting);
        
        var expected = await _context.Companies.Select(c => c.MapToCompanyDto()).ToListAsync();
        
        Assert.That(result.companies, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCorrectIdsString_WhenPassedCollectionWithEntries()
    {
        var testCompanies = await _context.Companies.ToListAsync();
        var companiesForInserting = testCompanies.Select(c => c.MapToCompanyForCreationDto());
        await _context.Database.EnsureDeletedAsync();

        var result = await _companyService.CompanyService.CreateCompanyCollection(companiesForInserting);
        
        var idsFromDb = await _context.Companies.Select(c => c.Id).ToListAsync();
        var expected = string.Join(',', idsFromDb);
        
        Assert.That(result.ids, Is.EquivalentTo(expected));
    }
    
    #endregion

    #region Delete Company Tests
    [Test]
    public async Task DeleteCompany_ThrowsCompanyNotFoundException_WhenIncorrectIdPassed()
    {
        var incorrectId = Guid.NewGuid();

        Assert.ThrowsAsync<CompanyNotFoundException>(async () =>
            await _companyService.CompanyService.DeleteCompany(incorrectId, trackChanges: false));
    }
    
    [Test]
    public async Task DeleteCompany_SuccessfullyDeletesCompany()
    {
        var testCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Test company name",
            Address = "Test address",
            Country = "Test country"
        };
        await _context.Database.EnsureDeletedAsync();
        await _context.Companies.AddAsync(testCompany);
        await _context.SaveChangesAsync();

        await _companyService.CompanyService.DeleteCompany(testCompany.Id, trackChanges: true);
        var result = await _context.Companies.AnyAsync(c => c.Id == testCompany.Id);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteCompany_SuccessfullyDeletesChildren()
    {
        var testCompany = await _context.Companies.FirstAsync();
        
        await _companyService.CompanyService.DeleteCompany(testCompany.Id, trackChanges: true);
        var result = await _context.Employees.AnyAsync(c => c.CompanyId == testCompany.Id);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteCompany_DoesNotDeleteOtherCompaniesChildren()
    {
        var testCompany = await _context.Companies.FirstAsync();
        var expected = await _context.Employees.Where(e => 
            e.CompanyId != testCompany.Id)
            .ToListAsync();
        
        await _companyService.CompanyService.DeleteCompany(testCompany.Id, trackChanges: true);
        var result = await _context.Employees.ToListAsync();
        
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    #endregion
    
    #region Update Company Tests
    
    [Test]
    public async Task UpdateCompany_ThrowsCompanyNotFoundException_WhenIncorrectCompanyId()
    {
        var companyForUpdate = new CompanyForUpdateDto{
            Name = "UpdatedName", 
            Address = "UpdatedAddress",
            Country = "UpdatedCountry", 
            Employees = []
        };
        var incorrectCompanyId = Guid.NewGuid();
        
        Assert.ThrowsAsync<CompanyNotFoundException>(async () =>
            await _companyService.CompanyService.UpdateCompany(incorrectCompanyId, companyForUpdate, trackChanges: true)); 
    }
    
    [Test]
    public async Task UpdateCompany_UpdatesName_WhenOnlyNameForChangePassed()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = "UpdatedName", 
            Address = string.Empty, 
            Country = string.Empty, 
            Employees = []
        };
        
        expected.Name = testUpdateDto.Name;

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_UpdatesAddress_WhenOnlyAddressForChangePassed()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = string.Empty, 
            Address = "UpdatedAddress",
            Country = string.Empty,
            Employees = []
        };
        
        expected.Address = testUpdateDto.Address;

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_UpdatesCountry_WhenOnlyCountryForChangePassed()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = string.Empty,
            Address = string.Empty,
            Country = "UpdatedCountry",
            Employees = []
        };
        
        expected.Country = testUpdateDto.Country;

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateEmployeeForCompany_UpdatesEmployees_WhenOnlyEmployeesForChangePassed()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = string.Empty,
            Address = string.Empty, 
            Country = string.Empty, 
            Employees = 
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" },
                new EmployeeForCreationDto{ Name = "Test2", Age = 2, Position = "Test2" }
            ]
        };

        expected.Employees = testUpdateDto!.Employees.Select(e => e.MapToEntity()).ToList();

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_Nulls()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto 
        {
            Name = null,
            Address = null, 
            Country = null, 
            Employees = null 
        };

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_EmptyStringEmptyCollection()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = string.Empty,
            Address = string.Empty,
            Country = string.Empty,
            Employees = []
        };

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_EmptyStringNullCollection()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = string.Empty,
            Address = string.Empty,
            Country = string.Empty,
            Employees = null
        };

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_WhitespaceEmptyCollection()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = " ",
            Address = " ",
            Country = " ",
            Employees = []
        };

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task UpdateCompany_DoesNotUpdatesEntry_WhenIncorrectArgumentsArePassed_WhitespaceNullCollection()
    {
        var expected = await _context.Companies.FirstAsync();
        var testUpdateDto = new CompanyForUpdateDto
        {
            Name = " ",
            Address = " ",
            Country = " ",
            Employees = null
        };

        await _companyService.CompanyService.UpdateCompany(expected.Id, testUpdateDto, trackChanges: true);
        var result = await _context.Companies.FirstAsync(e => e.Id.Equals(expected.Id));
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    #endregion
}