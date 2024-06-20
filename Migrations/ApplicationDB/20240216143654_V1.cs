using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUDApi.Migrations.ApplicationDB
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e3c4a99-c6c9-4b18-a9ef-2d8a326d0be2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ace9f3d-7674-4927-aca5-56aff8f493b0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b026790-3284-418e-989a-90bee616c8f0", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "e273baf4-e3df-4ec5-883c-b60f34f42283", null, "ApplicationUserRole", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "5e3c4a99-c6c9-4b18-a9ef-2d8a326d0be2", null, "ApplicationUserRole", "User", "User" },
                    { "7ace9f3d-7674-4927-aca5-56aff8f493b0", null, "ApplicationUserRole", "Admin", "Admin" }
                });
        }
    }
}
