using Contracts.Repository;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges) =>
        await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

    public async Task<Company?> GetCompany(Guid companyId, bool trackChanges) =>
        await FindByCondition(c => c.Id == companyId, trackChanges)
            .SingleOrDefaultAsync();

    public void CreateCompany(Company company) =>
        Create(company);
    
    public async Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
    
    public void DeleteCompany(Company company) => 
        Delete(company);

    public async Task<bool> CompanyExists(Guid companyId) =>
        await RepositoryContext.Companies.AnyAsync(c => c.Id.Equals(companyId));
}