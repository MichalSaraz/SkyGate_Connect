using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_PK_ApisData_to_Guid_step1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Countries_IssueCountryId",
                table: "APISData");

            migrationBuilder.RenameColumn(
                name: "IssueCountryId",
                table: "APISData",
                newName: "CountryOfIssueId");

            migrationBuilder.RenameIndex(
                name: "IX_APISData_IssueCountryId",
                table: "APISData",
                newName: "IX_APISData_CountryOfIssueId");

            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "APISData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Countries_CountryOfIssueId",
                table: "APISData",
                column: "CountryOfIssueId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Countries_CountryOfIssueId",
                table: "APISData");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "APISData");

            migrationBuilder.RenameColumn(
                name: "CountryOfIssueId",
                table: "APISData",
                newName: "IssueCountryId");

            migrationBuilder.RenameIndex(
                name: "IX_APISData_CountryOfIssueId",
                table: "APISData",
                newName: "IX_APISData_IssueCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Countries_IssueCountryId",
                table: "APISData",
                column: "IssueCountryId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");
        }
    }
}
