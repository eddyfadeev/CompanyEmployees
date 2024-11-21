using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasData
        (
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Indiana Jones",
                Position = "Archaeologist",
                CompanyId = new Guid("12345678-1234-1234-1234-123456789012")
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Lara Croft",
                Position = "Tomb Rider",
                CompanyId = new Guid("12345678-1234-1234-1234-123456789012")
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Jurgen Feller",
                Position = "NASA Scientist",
                CompanyId = new Guid("87654321-4321-4321-4321-210987654321")
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Mathias Vogel",
                Position = "Archaeologist",
                CompanyId = new Guid("87654321-4321-4321-4321-210987654321")
            }
        );
    }
}