using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UserAdres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "508082c1-bf01-4a7d-a344-e25a76918279");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3483e43-9af2-403a-822d-1c001d69fae9");

            migrationBuilder.RenameColumn(
                name: "SIFRE",
                table: "Users",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9cfc4b4c-e9a7-4d8e-8405-c81711472241", null, "User", "USER" },
                    { "c3eb197d-7eef-4cd3-b7b8-891ff6889e74", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cfc4b4c-e9a7-4d8e-8405-c81711472241");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3eb197d-7eef-4cd3-b7b8-891ff6889e74");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "SIFRE");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "508082c1-bf01-4a7d-a344-e25a76918279", null, "User", "USER" },
                    { "a3483e43-9af2-403a-822d-1c001d69fae9", null, "Admin", "ADMIN" }
                });
        }
    }
}
