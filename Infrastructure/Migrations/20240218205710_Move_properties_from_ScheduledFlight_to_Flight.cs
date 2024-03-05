using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Move_properties_from_ScheduledFlight_to_Flight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Airlines_AirlineId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationFromId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationToId",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_AirlineId",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_DestinationFromId",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_DestinationToId",
                table: "ScheduledFlights");

            migrationBuilder.AddColumn<string>(
                name: "FlightClass",
                table: "PassengerFlight",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AirlineId",
                table: "Flights",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationFromId",
                table: "Flights",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationToId",
                table: "Flights",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationFromId",
                table: "Flights",
                column: "DestinationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationToId",
                table: "Flights",
                column: "DestinationToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airlines_AirlineId",
                table: "Flights",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_DestinationFromId",
                table: "Flights",
                column: "DestinationFromId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_DestinationToId",
                table: "Flights",
                column: "DestinationToId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airlines_AirlineId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_DestinationFromId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_DestinationToId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DestinationFromId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DestinationToId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightClass",
                table: "PassengerFlight");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DestinationFromId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DestinationToId",
                table: "Flights");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_AirlineId",
                table: "ScheduledFlights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DestinationFromId",
                table: "ScheduledFlights",
                column: "DestinationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DestinationToId",
                table: "ScheduledFlights",
                column: "DestinationToId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Airlines_AirlineId",
                table: "ScheduledFlights",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationFromId",
                table: "ScheduledFlights",
                column: "DestinationFromId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationToId",
                table: "ScheduledFlights",
                column: "DestinationToId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");
        }
    }
}
