using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUDApi.Migrations.ApplicationDB
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b026790-3284-418e-989a-90bee616c8f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e273baf4-e3df-4ec5-883c-b60f34f42283");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8219037c-6ed2-4471-b0a9-d48ed6d75a5b", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "9fb19e80-98c6-4d42-a799-517338bcc493", null, "ApplicationUserRole", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8219037c-6ed2-4471-b0a9-d48ed6d75a5b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fb19e80-98c6-4d42-a799-517338bcc493");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b026790-3284-418e-989a-90bee616c8f0", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "e273baf4-e3df-4ec5-883c-b60f34f42283", null, "ApplicationUserRole", "User", "User" }
                });
        }
    }
}
