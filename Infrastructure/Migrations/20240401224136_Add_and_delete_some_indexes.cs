using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_and_delete_some_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FrequentFlyerCards_CardNumber",
                table: "FrequentFlyerCards");

            migrationBuilder.DropIndex(
               name: "IX_Passengers_Id",
               table: "Passengers");

            migrationBuilder.DropColumn(
                name: "FrequentFlyerNumber",
                table: "FrequentFlyerCards");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "Flights");

            migrationBuilder.CreateIndex(
                name: "IX_FlightBaggage_FlightId",
                table: "FlightBaggage",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightComment_FlightId",
                table: "FlightComment",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightComment_CommentId",
                table: "FlightComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightId",
                table: "Seats",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialServiceRequests_FlightId",
                table: "SpecialServiceRequests",
                column: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerFlight_PassengerId",
                table: "PassengerFlight",
                newName: "IX_PassengerFlight_BasePassengerOrItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FlightBaggage_FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropIndex(
                name: "IX_FlightComment_FlightId",
                table: "FlightComment");

            migrationBuilder.DropIndex(
                name: "IX_FlightComment_CommentId",
                table: "FlightComment");

            migrationBuilder.DropIndex(
                name: "IX_Seats_FlightId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_SpecialServiceRequests_FlightId",
                table: "SpecialServiceRequests");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerFlight_BasePassengerOrItemId",
                table: "PassengerFlight",
                newName: "IX_PassengerFlight_PassengerId");

            migrationBuilder.AddColumn<string>(
                name: "FrequentFlyerNumber",
                table: "FrequentFlyerCards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "Flights",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid());

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyerCards_CardNumber",
                table: "FrequentFlyerCards",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_Id",
                table: "Passengers",
                column: "Id",
                unique: true);
        }
    }
}