using System.Reflection;
using System.Text;

namespace Repository.Utility;

public static class OrderQueryBuilder
{
    public static string CreateOrderQuery<T>(string orderByQuery)
    {
        var orderParams = orderByQuery.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var queryBuilder = new StringBuilder();

        foreach (string param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                continue;
            }

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(info =>
                info.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty is null)
            {
                continue;
            }

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            
            queryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
        }

        var orderQuery = queryBuilder.ToString().TrimEnd(',', ' ');

        return orderQuery;
    }
}