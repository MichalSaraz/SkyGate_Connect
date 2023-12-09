using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class minor_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ScheduledFlightFlightNumber",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "ScheduledFlightNumber",
                table: "Flights",
                newName: "ScheduledFlightId");

            migrationBuilder.RenameColumn(
                name: "ScheduledFlightFlightNumber",
                table: "Flights",
                newName: "AircraftRegistrationCode");

            migrationBuilder.AddColumn<string>(
                name: "FlightId",
                table: "Aircrafts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftRegistrationCode",
                table: "Flights",
                column: "AircraftRegistrationCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ScheduledFlightId",
                table: "Flights",
                column: "ScheduledFlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftRegistrationCode",
                table: "Flights",
                column: "AircraftRegistrationCode",
                principalTable: "Aircrafts",
                principalColumn: "RegistrationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_ScheduledFlights_ScheduledFlightId",
                table: "Flights",
                column: "ScheduledFlightId",
                principalTable: "ScheduledFlights",
                principalColumn: "FlightNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftRegistrationCode",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_ScheduledFlights_ScheduledFlightId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AircraftRegistrationCode",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ScheduledFlightId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Aircrafts");

            migrationBuilder.RenameColumn(
                name: "ScheduledFlightId",
                table: "Flights",
                newName: "ScheduledFlightNumber");

            migrationBuilder.RenameColumn(
                name: "AircraftRegistrationCode",
                table: "Flights",
                newName: "ScheduledFlightFlightNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ScheduledFlightFlightNumber",
                table: "Flights",
                column: "ScheduledFlightFlightNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flights",
                column: "ScheduledFlightFlightNumber",
                principalTable: "ScheduledFlights",
                principalColumn: "FlightNumber");
        }
    }
}
