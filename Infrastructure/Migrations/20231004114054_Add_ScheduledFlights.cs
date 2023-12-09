using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ScheduledFlights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Flights_FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airlines_AirlineId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_ArrivalAirportId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_DepartureAirportId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Flights_FlightId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatMaps_SeatMapId",
                table: "Seat");

            migrationBuilder.DropTable(
                name: "Codeshares");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ArrivalAirportId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DepartureAirportId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "SeatMapId",
                table: "SeatMaps");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DepartureAirportId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightIdentifier",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "Flights");

            migrationBuilder.RenameTable(
                name: "Flights",
                newName: "Flight");

            migrationBuilder.RenameColumn(
                name: "SeatMapId",
                table: "Seat",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_SeatMapId",
                table: "Seat",
                newName: "IX_Seat_FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_AircraftId",
                table: "Flight",
                newName: "IX_Flight_AircraftId");

            migrationBuilder.AlterColumn<string>(
                name: "BoardingStatus",
                table: "Flight",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "DividerPlacedBehindRow",
                table: "Flight",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledFlightId",
                table: "Flight",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flight",
                table: "Flight",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ScheduledFlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightNumber = table.Column<string>(type: "text", nullable: true),
                    FlightIdentifier = table.Column<int>(type: "integer", nullable: false),
                    Codeshare = table.Column<string[]>(type: "text[]", nullable: true),
                    DepartureTimes = table.Column<string>(type: "jsonb", nullable: true),
                    DepartureAirportId = table.Column<int>(type: "integer", nullable: false),
                    ArrivalAirportId = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledFlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledFlights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledFlights_Destinations_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledFlights_Destinations_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_ScheduledFlightId",
                table: "Flight",
                column: "ScheduledFlightId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_AirlineId",
                table: "ScheduledFlights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_ArrivalAirportId",
                table: "ScheduledFlights",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DepartureAirportId",
                table: "ScheduledFlights",
                column: "DepartureAirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_ScheduledFlights_ScheduledFlightId",
                table: "Flight",
                column: "ScheduledFlightId",
                principalTable: "ScheduledFlights",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Flight_FlightId",
                table: "Seat",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight");

            migrationBuilder.DropForeignKey(
                name: "FK_Flight_ScheduledFlights_ScheduledFlightId",
                table: "Flight");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Flight_FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Flight_FlightId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Flight_FlightId",
                table: "Seat");

            migrationBuilder.DropTable(
                name: "ScheduledFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flight",
                table: "Flight");

            migrationBuilder.DropIndex(
                name: "IX_Flight_ScheduledFlightId",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "DividerPlacedBehindRow",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "ScheduledFlightId",
                table: "Flight");

            migrationBuilder.RenameTable(
                name: "Flight",
                newName: "Flights");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "Seat",
                newName: "SeatMapId");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_FlightId",
                table: "Seat",
                newName: "IX_Seat_SeatMapId");

            migrationBuilder.RenameIndex(
                name: "IX_Flight_AircraftId",
                table: "Flights",
                newName: "IX_Flights_AircraftId");

            migrationBuilder.AddColumn<string>(
                name: "SeatMapId",
                table: "SeatMaps",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BoardingStatus",
                table: "Flights",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArrivalAirportId",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartureAirportId",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightIdentifier",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flights",
                table: "Flights",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Codeshares",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeshareAirlineId = table.Column<int>(type: "integer", nullable: false),
                    CodeshareFlightIdentifier = table.Column<int>(type: "integer", nullable: false),
                    CodeshareFlightNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codeshares", x => new { x.FlightId, x.Id });
                    table.ForeignKey(
                        name: "FK_Codeshares_Airlines_CodeshareAirlineId",
                        column: x => x.CodeshareAirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Codeshares_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ArrivalAirportId",
                table: "Flights",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureAirportId",
                table: "Flights",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Codeshares_CodeshareAirlineId",
                table: "Codeshares",
                column: "CodeshareAirlineId");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airlines_AirlineId",
                table: "Flights",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_ArrivalAirportId",
                table: "Flights",
                column: "ArrivalAirportId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_DepartureAirportId",
                table: "Flights",
                column: "DepartureAirportId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Flights_FlightId",
                table: "PassengerFlight",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatMaps_SeatMapId",
                table: "Seat",
                column: "SeatMapId",
                principalTable: "SeatMaps",
                principalColumn: "Id");
        }
    }
}
