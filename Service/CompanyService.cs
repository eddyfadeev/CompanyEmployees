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

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = 
            _repository.Company.GetAllCompanies(trackChanges);

        var companiesDto =
            companies.Select(c => c.MapToCompanyDto()).ToList();
            
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(id, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(id);
        }
        
        return company.MapToCompanyDto();
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = company.MapToEntity();

        _repository.Company.CreateCompany(companyEntity);
        _repository.Save();
        
        return companyEntity.MapToCompanyDto();
    }
    
    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid>? ids, bool trackChanges) 
    {
        if (ids is null)
        {
            throw new IdParametersBadRequestException();
        }
        var companyIds = ids.ToList();
        
        var companies = _repository.Company.GetByIds(companyIds, trackChanges).ToList();
        if (companyIds.Count != companies.Count)
        {
            throw new CollectionByIdsBadRequestException();
        }

        var companiesToReturn = companies.Select(c => c.MapToCompanyDto());

        return companiesToReturn;
    }

    
    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
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
        
        _repository.Save();
        
        var companyCollectionToReturn = companyList.Select(c => c.MapToCompanyDto()).ToList();
        var ids = string.Join(',', companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids);
    }
}