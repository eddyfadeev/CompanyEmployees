using Entities.Models;

namespace Repository.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
        employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

    public static IQueryable<Employee> SearchByName(this IQueryable<Employee> employees, string searchItem) =>
        string.IsNullOrWhiteSpace(searchItem) 
            ? employees 
            : employees.Where(e => 
                e.Name != null && e.Name.Contains(searchItem.Trim().ToLower())
                );
}