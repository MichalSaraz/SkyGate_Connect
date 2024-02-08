using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_PassengerInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerInfos_BookingReferences_PNRId",
                table: "PassengerInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerInfos_Id",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassengerInfos",
                table: "PassengerInfos");

            migrationBuilder.RenameTable(
                name: "PassengerInfos",
                newName: "PassengerInfo");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerInfos_PNRId",
                table: "PassengerInfo",
                newName: "IX_PassengerInfo_PNRId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassengerInfo",
                table: "PassengerInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_PassengerInfo_PassengerInfoId",
                table: "FrequentFlyers",
                column: "PassengerInfoId",
                principalTable: "PassengerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerInfo_BookingReferences_PNRId",
                table: "PassengerInfo",
                column: "PNRId",
                principalTable: "BookingReferences",
                principalColumn: "PNR",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerInfo_Id",
                table: "Passengers",
                column: "Id",
                principalTable: "PassengerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_PassengerInfo_PassengerInfoId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerInfo_BookingReferences_PNRId",
                table: "PassengerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerInfo_Id",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassengerInfo",
                table: "PassengerInfo");

            migrationBuilder.RenameTable(
                name: "PassengerInfo",
                newName: "PassengerInfos");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerInfo_PNRId",
                table: "PassengerInfos",
                newName: "IX_PassengerInfos_PNRId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassengerInfos",
                table: "PassengerInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
                table: "FrequentFlyers",
                column: "PassengerInfoId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerInfos_BookingReferences_PNRId",
                table: "PassengerInfos",
                column: "PNRId",
                principalTable: "BookingReferences",
                principalColumn: "PNR",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerInfos_Id",
                table: "Passengers",
                column: "Id",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
