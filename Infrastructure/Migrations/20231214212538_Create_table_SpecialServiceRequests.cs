using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_table_SpecialServiceRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Passengers_PassengerId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequest_Flights_FlightId",
                table: "SpecialServiceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequest_Passengers_PassengerId",
                table: "SpecialServiceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequest_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialServiceRequest",
                table: "SpecialServiceRequest");

            migrationBuilder.RenameTable(
                name: "SpecialServiceRequest",
                newName: "SpecialServiceRequests");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceRequest_SSRCodeId",
                table: "SpecialServiceRequests",
                newName: "IX_SpecialServiceRequests_SSRCodeId");

            migrationBuilder.DropIndex(
                name: "IX_SpecialServiceRequest_PassengerId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "SpecialServiceRequests");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceRequest_FlightId",
                table: "SpecialServiceRequests",
                newName: "IX_SpecialServiceRequests_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialServiceRequests",
                table: "SpecialServiceRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Passengers_PassengerId",
                table: "Seats",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequests_Flights_FlightId",
                table: "SpecialServiceRequests",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequests",
                column: "SSRCodeId",
                principalTable: "SSRCodes",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        /// not complete
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Passengers_PassengerId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_Flights_FlightId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_Passengers_PassengerId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialServiceRequests",
                table: "SpecialServiceRequests");

            migrationBuilder.RenameTable(
                name: "SpecialServiceRequests",
                newName: "SpecialServiceRequest");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceRequests_SSRCodeId",
                table: "SpecialServiceRequest",
                newName: "IX_SpecialServiceRequest_SSRCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceRequests_PassengerId",
                table: "SpecialServiceRequest",
                newName: "IX_SpecialServiceRequest_PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialServiceRequests_FlightId",
                table: "SpecialServiceRequest",
                newName: "IX_SpecialServiceRequest_FlightId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassengerId",
                table: "Seats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialServiceRequest",
                table: "SpecialServiceRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Passengers_PassengerId",
                table: "Seats",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequest_Flights_FlightId",
                table: "SpecialServiceRequest",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequest_Passengers_PassengerId",
                table: "SpecialServiceRequest",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequest_SSRCodes_SSRCodeId",
                table: "SpecialServiceRequest",
                column: "SSRCodeId",
                principalTable: "SSRCodes",
                principalColumn: "Code");
        }
    }
}
