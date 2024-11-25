using Entities.Models;

namespace Service.Tests;

internal class SeedDataProvider
{
    public SeedDataProvider()
    {
        Companies = GetTestCompanies();
        Employees = GetTestEmployees(Companies.ToList());
    }
    
    public IEnumerable<Company> Companies { get; }
    public IEnumerable<Employee> Employees { get; }

    private static IEnumerable<Company> GetTestCompanies() =>
    [
        new()
        {
            Id = new Guid("F57E7805-2C61-4FC7-A715-93339FF062C2"),
            Name = "Test Company A",
            Address = "Test Address A",
            Country = "Test Country A"
        },
        new()
        {
            Id = new Guid("82995975-23BF-4AE8-AD48-8DE919A863A8"),
            Name = "Test Company B",
            Address = "Test Address B",
            Country = "Test Country B"
        },
        new()
        {
            Id = new Guid("D83C96D1-5EBC-4D3F-887F-7B289F4A4621"),
            Name = "Test Company C",
            Address = "Test Address C",
            Country = "Test Country C"
        }
    ];
    
    private static IEnumerable<Employee> GetTestEmployees(List<Company> companies) =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee A1",
            Age = 30,
            Position = "Developer",
            CompanyId = companies[0].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee A2",
            Age = 35,
            Position = "Manager",
            CompanyId = companies[0].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee A3",
            Age = 40,
            Position = "Director",
            CompanyId = companies[0].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee B1",
            Age = 25,
            Position = "Developer",
            CompanyId = companies[1].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee B2",
            Age = 30,
            Position = "Manager",
            CompanyId = companies[1].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee B3",
            Age = 35,
            Position = "Director",
            CompanyId = companies[1].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee C1",
            Age = 28,
            Position = "Developer",
            CompanyId = companies[2].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee C2",
            Age = 33,
            Position = "Manager",
            CompanyId = companies[2].Id
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Employee C3",
            Age = 38,
            Position = "Director",
            CompanyId = companies[2].Id
        }
    ];
}