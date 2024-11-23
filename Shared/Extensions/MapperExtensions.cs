using Entities.Models;
using Shared.DTO;

namespace Shared.Extensions;

public static class MapperExtensions
{
    public static CompanyDto MapToDto(this Company company) =>
        new
        (
            id: company.Id,
            name: company.Name ?? string.Empty,
            address: company.Address ?? string.Empty,
            country: company.Country ?? string.Empty
        );
    
    public static Company MapToEntity(this CompanyDto company)
    {
        company.Deconstruct(out var id, out var name, out var address, out var country);
        
        return new Company
        {
            Id = id,
            Name = name,
            Address = address,
            Country = country
        };
    }
}