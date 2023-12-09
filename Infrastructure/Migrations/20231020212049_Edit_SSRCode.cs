using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_SSRCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SSRCodes_PassengerInfos_PassengerInfoId",
                table: "SSRCodes");

            migrationBuilder.DropIndex(
                name: "IX_SSRCodes_PassengerInfoId",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "FreeText",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "PassengerInfoId",
                table: "SSRCodes");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFreeTextMandatory",
                table: "SSRCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SSRList",
                table: "PassengerInfos",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SSRList",
                table: "PassengerInfos");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFreeTextMandatory",
                table: "SSRCodes",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "SSRCodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FreeText",
                table: "SSRCodes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengerInfoId",
                table: "SSRCodes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SSRCodes_PassengerInfoId",
                table: "SSRCodes",
                column: "PassengerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SSRCodes_PassengerInfos_PassengerInfoId",
                table: "SSRCodes",
                column: "PassengerInfoId",
                principalTable: "PassengerInfos",
                principalColumn: "Id");
        }
    }
}
