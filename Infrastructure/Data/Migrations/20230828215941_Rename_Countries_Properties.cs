using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Countries_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountryCodeShort",
                table: "Countries",
                newName: "Country3LetterCode");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "Countries",
                newName: "Country2LetterCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country3LetterCode",
                table: "Countries",
                newName: "CountryCodeShort");

            migrationBuilder.RenameColumn(
                name: "Country2LetterCode",
                table: "Countries",
                newName: "CountryCode");
        }
    }
}
