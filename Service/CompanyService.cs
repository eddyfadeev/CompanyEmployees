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
            companies.Select(c => c.MapToDto()).ToList();
            
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(id, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(id);
        }
        
        return company.MapToDto();
    }
}