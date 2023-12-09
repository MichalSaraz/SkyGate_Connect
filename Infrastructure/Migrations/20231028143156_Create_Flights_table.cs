using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_Flights_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight");

            migrationBuilder.DropForeignKey(
                name: "FK_Flight_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flight");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Flight_FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Flight_FlightId",
                table: "PassengerFlight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flight",
                table: "Flight");

            migrationBuilder.RenameTable(
                name: "Flight",
                newName: "Flights");

            migrationBuilder.RenameIndex(
                name: "IX_Flight_ScheduledFlightFlightNumber",
                table: "Flights",
                newName: "IX_Flights_ScheduledFlightFlightNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Flight_AircraftId",
                table: "Flights",
                newName: "IX_Flights_AircraftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flights",
                table: "Flights",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightBaggage_Flights_FlightId",
                table: "FlightBaggage",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftId",
                table: "Flights",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "RegistrationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flights",
                column: "ScheduledFlightFlightNumber",
                principalTable: "ScheduledFlights",
                principalColumn: "FlightNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Flights_FlightId",
                table: "PassengerFlight",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Flights_FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Flights_FlightId",
                table: "PassengerFlight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");

            migrationBuilder.RenameTable(
                name: "Flights",
                newName: "Flight");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_ScheduledFlightFlightNumber",
                table: "Flight",
                newName: "IX_Flight_ScheduledFlightFlightNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_AircraftId",
                table: "Flight",
                newName: "IX_Flight_AircraftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flight",
                table: "Flight",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "RegistrationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flight",
                column: "ScheduledFlightFlightNumber",
                principalTable: "ScheduledFlights",
                principalColumn: "FlightNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightBaggage_Flight_FlightId",
                table: "FlightBaggage",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Flight_FlightId",
                table: "PassengerFlight",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
