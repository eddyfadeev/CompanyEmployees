using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class Second_SeedDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("12345678-1234-1234-1234-123456789012"), "666 SomewhereInUS Rd., New York, NY, 10001", "USA", "Suspicious Solutions Ltd" },
                    { new Guid("87654321-4321-4321-4321-210987654321"), "777 SomewhereInCanada Blvd., Edmonton, AB, T6W 1T7", "Canada", "Kinda Suspicious Solutions Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("2af845da-d4e1-4e3a-9c31-45e17cd75c2e"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Mathias Vogel", "Archaeologist" },
                    { new Guid("2de4ebfc-9b51-42d7-acb8-ba91ffcece47"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Lara Croft", "Tomb Rider" },
                    { new Guid("4b276462-ab30-4c60-803d-f93432087f19"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Jurgen Feller", "NASA Scientist" },
                    { new Guid("d1de6b78-c874-472e-b664-9012ef42287b"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Indiana Jones", "Archaeologist" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2af845da-d4e1-4e3a-9c31-45e17cd75c2e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2de4ebfc-9b51-42d7-acb8-ba91ffcece47"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("4b276462-ab30-4c60-803d-f93432087f19"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d1de6b78-c874-472e-b664-9012ef42287b"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("12345678-1234-1234-1234-123456789012"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("87654321-4321-4321-4321-210987654321"));
        }
    }
}
