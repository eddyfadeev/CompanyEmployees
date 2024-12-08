using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class Configured_Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("03806c60-7201-4b2f-9e54-2829c7948dbf"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("50d37287-611d-4021-9f41-bf2ac9c558bd"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("b91e7411-6b67-4843-a0ef-45c6f5635797"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d35ed872-fdf7-4b71-b735-7c236f423c98"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "012985c0-ab54-4019-835f-70a7b2126c96", null, "Manager", "MANAGER" },
                    { "43a17aad-0e87-48ef-80b6-f6effbc7ef57", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("19708463-599e-42aa-95e5-3b5c19891316"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Lara Croft", "Tomb Rider" },
                    { new Guid("2b19fa35-507a-49c5-b3a1-4c0c8546d738"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Jurgen Feller", "NASA Scientist" },
                    { new Guid("68845530-a4fa-4b24-b046-c73993942c9a"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Mathias Vogel", "Archaeologist" },
                    { new Guid("c39b33e6-2ea2-471c-9117-0b5fe614e951"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Indiana Jones", "Archaeologist" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "012985c0-ab54-4019-835f-70a7b2126c96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43a17aad-0e87-48ef-80b6-f6effbc7ef57");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("19708463-599e-42aa-95e5-3b5c19891316"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2b19fa35-507a-49c5-b3a1-4c0c8546d738"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("68845530-a4fa-4b24-b046-c73993942c9a"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("c39b33e6-2ea2-471c-9117-0b5fe614e951"));

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("03806c60-7201-4b2f-9e54-2829c7948dbf"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Mathias Vogel", "Archaeologist" },
                    { new Guid("50d37287-611d-4021-9f41-bf2ac9c558bd"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Indiana Jones", "Archaeologist" },
                    { new Guid("b91e7411-6b67-4843-a0ef-45c6f5635797"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Jurgen Feller", "NASA Scientist" },
                    { new Guid("d35ed872-fdf7-4b71-b735-7c236f423c98"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Lara Croft", "Tomb Rider" }
                });
        }
    }
}
