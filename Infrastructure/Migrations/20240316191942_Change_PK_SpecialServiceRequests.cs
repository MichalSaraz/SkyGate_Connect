using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_PK_SpecialServiceRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialServiceRequests",
                table: "SpecialServiceRequests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SpecialServiceRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialServiceRequests",
                table: "SpecialServiceRequests",
                column: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SpecialServiceRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
