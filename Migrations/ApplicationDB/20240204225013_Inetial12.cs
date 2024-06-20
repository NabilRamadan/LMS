using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUDApi.Migrations.ApplicationDB
{
    /// <inheritdoc />
    public partial class Inetial12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92ce88cd-b22a-4c94-b922-23fafcca6632");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5299e43-2c40-4a6d-a45c-e0be0a4f5693");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0cc95554-e23a-415a-af87-522adfafe42d", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "14de7b17-cbbf-4515-b296-650ff59d5680", null, "ApplicationUserRole", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cc95554-e23a-415a-af87-522adfafe42d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14de7b17-cbbf-4515-b296-650ff59d5680");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92ce88cd-b22a-4c94-b922-23fafcca6632", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "a5299e43-2c40-4a6d-a45c-e0be0a4f5693", null, "ApplicationUserRole", "User", "User" }
                });
        }
    }
}
