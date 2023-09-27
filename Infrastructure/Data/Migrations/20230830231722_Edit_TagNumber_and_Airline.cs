using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_TagNumber_and_Airline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirlineCode",
                table: "TagNumbers");

            migrationBuilder.RenameColumn(
                name: "BagTagIssuerCode",
                table: "TagNumbers",
                newName: "AirlineId");

            migrationBuilder.AddColumn<int>(
                name: "AccountingCode",
                table: "Airlines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirlinePrefix",
                table: "Airlines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Airlines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TagNumbers_AirlineId",
                table: "TagNumbers",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_CountryId",
                table: "Airlines",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airlines_Countries_CountryId",
                table: "Airlines",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNumbers_Airlines_AirlineId",
                table: "TagNumbers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airlines_Countries_CountryId",
                table: "Airlines");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNumbers_Airlines_AirlineId",
                table: "TagNumbers");

            migrationBuilder.DropIndex(
                name: "IX_TagNumbers_AirlineId",
                table: "TagNumbers");

            migrationBuilder.DropIndex(
                name: "IX_Airlines_CountryId",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "AccountingCode",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "AirlinePrefix",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Airlines");

            migrationBuilder.RenameColumn(
                name: "AirlineId",
                table: "TagNumbers",
                newName: "BagTagIssuerCode");

            migrationBuilder.AddColumn<string>(
                name: "AirlineCode",
                table: "TagNumbers",
                type: "text",
                nullable: true);
        }
    }
}
