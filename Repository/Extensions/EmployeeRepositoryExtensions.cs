using System.Linq.Dynamic.Core;
using Entities.Models;
using Repository.Utility;

namespace Repository.Extensions;

public static class EmployeeRepositoryExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
        employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

    public static IQueryable<Employee> SearchByName(this IQueryable<Employee> employees, string searchItem) =>
        string.IsNullOrWhiteSpace(searchItem) 
            ? employees 
            : employees.Where(e => 
                e.Name != null && e.Name.Contains(searchItem.Trim().ToLower())
                );
    
    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQuery)
    {
        if (string.IsNullOrWhiteSpace(orderByQuery))
        {
            return employees.OrderBy(e => e.Name);
        }

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQuery);
        
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return employees.OrderBy(e => e.Name);
        }

        return employees.OrderBy(orderQuery);
    }
}