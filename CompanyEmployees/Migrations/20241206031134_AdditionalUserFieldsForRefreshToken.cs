using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalUserFieldsForRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a0307200-fb5f-40e2-b2fd-123a4e1cfeab", null, "Manager", "MANAGER" },
                    { "de6806d6-9f64-4e62-ae3b-5540c9bda8c4", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("19cc77db-ca82-4566-b0e3-ecd99f1cc33d"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Indiana Jones", "Archaeologist" },
                    { new Guid("39534dd1-72e3-4dfd-98d6-08da482b075b"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Jurgen Feller", "NASA Scientist" },
                    { new Guid("6a267b6d-4131-4cb2-ab5d-87d7ed492bb9"), 0, new Guid("87654321-4321-4321-4321-210987654321"), "Mathias Vogel", "Archaeologist" },
                    { new Guid("e5da6688-886c-4adc-a6dd-adac23897117"), 0, new Guid("12345678-1234-1234-1234-123456789012"), "Lara Croft", "Tomb Rider" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0307200-fb5f-40e2-b2fd-123a4e1cfeab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de6806d6-9f64-4e62-ae3b-5540c9bda8c4");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("19cc77db-ca82-4566-b0e3-ecd99f1cc33d"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("39534dd1-72e3-4dfd-98d6-08da482b075b"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("6a267b6d-4131-4cb2-ab5d-87d7ed492bb9"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e5da6688-886c-4adc-a6dd-adac23897117"));

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
