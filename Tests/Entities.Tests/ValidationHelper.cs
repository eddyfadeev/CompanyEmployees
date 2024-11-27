using System.ComponentModel.DataAnnotations;

namespace Entities.Tests;

public static class ValidationHelper
{
    public static IList<ValidationResult> ValidateObject(object obj)
    {
        var context = new ValidationContext(obj, null, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(obj, context, results, true);
        return results;
    }
}