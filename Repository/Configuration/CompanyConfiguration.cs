using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasData
        (
            new Company
            {
                Id = new Guid("12345678-1234-1234-1234-123456789012"),
                Name = "Suspicious Solutions Ltd",
                Address = "666 SomewhereInUS Rd., New York, NY, 10001",
                Country = "USA"
            },
            new Company
            {
                Id = new Guid("87654321-4321-4321-4321-210987654321"),
                Name = "Kinda Suspicious Solutions Ltd",
                Address = "777 SomewhereInCanada Blvd., Edmonton, AB, T6W 1T7",
                Country = "Canada"
            }
        );
    }
}