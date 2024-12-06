using Entities.Models;

namespace Contracts.Repository;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
    Task<Company?> GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteCompany(Company company);
    Task<bool> CompanyExists(Guid companyId);
}