using Entities.Models;

namespace Contracts.Repository;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Company? GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
}