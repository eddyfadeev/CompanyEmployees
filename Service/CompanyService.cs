using Contracts.Repository;
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
}