using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUDApi.Migrations.ApplicationDB
{
    /// <inheritdoc />
    public partial class Inetial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43f89742-bc10-4e12-ae7f-8d4131c51edb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c638f5d7-d18a-4a14-bf62-751343126b69");

            migrationBuilder.AddColumn<string>(
                name: "rol",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92ce88cd-b22a-4c94-b922-23fafcca6632", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "a5299e43-2c40-4a6d-a45c-e0be0a4f5693", null, "ApplicationUserRole", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92ce88cd-b22a-4c94-b922-23fafcca6632");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5299e43-2c40-4a6d-a45c-e0be0a4f5693");

            migrationBuilder.DropColumn(
                name: "rol",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43f89742-bc10-4e12-ae7f-8d4131c51edb", null, "ApplicationUserRole", "Admin", "Admin" },
                    { "c638f5d7-d18a-4a14-bf62-751343126b69", null, "ApplicationUserRole", "User", "User" }
                });
        }
    }
}
