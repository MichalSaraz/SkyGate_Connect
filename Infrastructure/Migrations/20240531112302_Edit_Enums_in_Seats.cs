using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_Enums_in_Seats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Seats",
                newName: "Letter");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Seats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "Letter",
                table: "Seats",
                newName: "Position");
        }
    }
}
