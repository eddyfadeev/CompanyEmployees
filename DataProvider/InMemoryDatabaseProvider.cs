using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace DataProvider;

public static class InMemoryDatabaseProvider
{
    public static async Task<RepositoryContext> CreateDatabaseContext(IEnumerable<Company> companies, IEnumerable<Employee> employees)
    { 
        var context = GetInMemoryDbContext();

        await context.Companies.AddRangeAsync(companies);
        await context.Employees.AddRangeAsync(employees);
        await context.SaveChangesAsync();
        
        return context;
    }
    
    private static RepositoryContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(
                databaseName: $"TestDb_{Guid.NewGuid().ToString()}")
            .Options;

        return new RepositoryContext(options);
    }
}