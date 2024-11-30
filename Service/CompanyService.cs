using Contracts.Repository;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DTO;
using Shared.Extensions;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;

    public CompanyService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges)
    {
        var companies = await _repository.Company.GetAllCompanies(trackChanges);

        var companiesDto =
            companies.Select(c => c.MapToCompanyDto()).ToList();
            
        return companiesDto;
    }

    public async Task<CompanyDto> GetCompany(Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(id, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(id);
        }
        
        return company.MapToCompanyDto();
    }

    public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = company.MapToEntity();

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();
        
        return companyEntity.MapToCompanyDto();
    }
    
    public async Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid>? ids, bool trackChanges) 
    {
        if (ids is null)
        {
            throw new IdParametersBadRequestException();
        }
        var companyIds = ids.ToList();
        
        var rawCompanies = await _repository.Company.GetByIds(companyIds, trackChanges);
        var companies = rawCompanies.ToList();
        if (companyIds.Count != companies.Count)
        {
            throw new CollectionByIdsBadRequestException();
        }

        var companiesToReturn = companies.Select(c => c.MapToCompanyDto());

        return companiesToReturn;
    }

    
    public async Task<(IEnumerable<CompanyDto> companies, string ids)> 
        CreateCompanyCollection(IEnumerable<CompanyForCreationDto>? companyCollection)
    {
        if (companyCollection is null)
        {
            throw new CompanyCollectionBadRequestException();
        }

        var companies = companyCollection.Select(c => c.MapToEntity());
        var companyList = companies.ToList();
        foreach (var company in companyList)
        {
            _repository.Company.CreateCompany(company);
        }
        
        await _repository.SaveAsync();
        
        var companyCollectionToReturn = companyList.Select(c => c.MapToCompanyDto()).ToList();
        var ids = string.Join(',', companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids);
    }

    public async Task DeleteCompany(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
        
        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        company.UpdateEntity(companyForUpdate);
        await _repository.SaveAsync();
    }
}