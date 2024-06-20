using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUDApi.Migrations.ApplicationDB
{
    /// <inheritdoc />
    public partial class Inetial123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cc95554-e23a-415a-af87-522adfafe42d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14de7b17-cbbf-4515-b296-650ff59d5680");

            migrationBuilder.RenameColumn(
                name: "rol",
                table: "AspNetUsers",
                newName: "Role");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "73cf1158-251c-4410-86f6-3d72a7649bc7", null, "ApplicationUserRole", "User", "User" },
                    { "c41e86a2-42fc-45ef-b643-3e5221077006", null, "ApplicationUserRole", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73cf1158-251c-4410-86f6-3d72a7649bc7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c41e86a2-42fc-45ef-b643-3e5221077006");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "rol");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0cc95554-e23a-415a-af87-522adfafe42d", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "14de7b17-cbbf-4515-b296-650ff59d5680", null, "ApplicationUserRole", "User", "User" }
                });
        }
    }
}
