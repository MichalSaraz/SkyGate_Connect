using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Project_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropColumn(
                name: "TotalBookedPassengers",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TotalCheckedBaggage",
                table: "Flights");

            migrationBuilder.AlterColumn<string>(
                name: "SSRCodeId",
                table: "SpecialServiceRequests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FrequentFlyerId",
                table: "PassengerInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookedClass",
                table: "PassengerInfo",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequests",
                column: "SSRCodeId",
                principalTable: "SSRCodes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropColumn(
                name: "BookedClass",
                table: "PassengerInfo");

            migrationBuilder.AlterColumn<string>(
                name: "SSRCodeId",
                table: "SpecialServiceRequests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "FrequentFlyerId",
                table: "PassengerInfo",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "TotalBookedPassengers",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCheckedBaggage",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequests",
                column: "SSRCodeId",
                principalTable: "SSRCodes",
                principalColumn: "Code");
        }
    }
}
