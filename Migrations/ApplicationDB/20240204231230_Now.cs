using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUDApi.Migrations.ApplicationDB
{
    /// <inheritdoc />
    public partial class Now : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cb4da7e-89bc-4c5e-a22e-22679c04a356");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9c15865-9987-4eff-b654-40336623e3de");

            migrationBuilder.AddColumn<string>(
                name: "CurrentUserRole",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e3c4a99-c6c9-4b18-a9ef-2d8a326d0be2", null, "ApplicationUserRole", "User", "User" },
                    { "7ace9f3d-7674-4927-aca5-56aff8f493b0", null, "ApplicationUserRole", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e3c4a99-c6c9-4b18-a9ef-2d8a326d0be2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ace9f3d-7674-4927-aca5-56aff8f493b0");

            migrationBuilder.DropColumn(
                name: "CurrentUserRole",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7cb4da7e-89bc-4c5e-a22e-22679c04a356", null, "ApplicationUserRole", "User", "User" },
                    { "c9c15865-9987-4eff-b654-40336623e3de", null, "ApplicationUserRole", "Admin", "Admin" }
                });
        }
    }
}
