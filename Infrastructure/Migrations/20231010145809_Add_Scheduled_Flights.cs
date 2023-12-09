using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Scheduled_Flights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledArrivals");

            migrationBuilder.DropTable(
                name: "ScheduledDepartures");

            migrationBuilder.CreateTable(
                name: "ScheduledFlights",
                columns: table => new
                {
                    FlightNumber = table.Column<string>(type: "text", nullable: false),
                    Codeshare = table.Column<string[]>(type: "text[]", nullable: true),
                    ScheduledTimes = table.Column<string>(type: "jsonb", nullable: true),
                    DestinationFromIATACode = table.Column<string>(type: "text", nullable: true),
                    DestinationToIATACode = table.Column<string>(type: "text", nullable: true),
                    AirlineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledFlights", x => x.FlightNumber);
                    table.ForeignKey(
                        name: "FK_ScheduledFlights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledFlights_Destinations_DestinationFromIATACode",
                        column: x => x.DestinationFromIATACode,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                    table.ForeignKey(
                        name: "FK_ScheduledFlights_Destinations_DestinationToIATACode",
                        column: x => x.DestinationToIATACode,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_AirlineId",
                table: "ScheduledFlights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DestinationFromIATACode",
                table: "ScheduledFlights",
                column: "DestinationFromIATACode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DestinationToIATACode",
                table: "ScheduledFlights",
                column: "DestinationToIATACode");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flight",
                column: "ScheduledFlightFlightNumber",
                principalTable: "ScheduledFlights",
                principalColumn: "FlightNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_ScheduledFlights_ScheduledFlightFlightNumber",
                table: "Flight");

            migrationBuilder.DropTable(
                name: "ScheduledFlights");

            migrationBuilder.CreateTable(
                name: "ScheduledArrivals",
                columns: table => new
                {
                    FlightNumber = table.Column<string>(type: "text", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    DestinationFromIATACode = table.Column<string>(type: "text", nullable: true),
                    DestinationToIATACode = table.Column<string>(type: "text", nullable: true),
                    Codeshare = table.Column<string[]>(type: "text[]", nullable: true),
                    ScheduledTimes = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledArrivals", x => x.FlightNumber);
                    table.ForeignKey(
                        name: "FK_ScheduledArrivals_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledArrivals_Destinations_DestinationFromIATACode",
                        column: x => x.DestinationFromIATACode,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                    table.ForeignKey(
                        name: "FK_ScheduledArrivals_Destinations_DestinationToIATACode",
                        column: x => x.DestinationToIATACode,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                });

            migrationBuilder.CreateTable(
                name: "ScheduledDepartures",
                columns: table => new
                {
                    FlightNumber = table.Column<string>(type: "text", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    DestinationFromIATACode = table.Column<string>(type: "text", nullable: true),
                    DestinationToIATACode = table.Column<string>(type: "text", nullable: true),
                    Codeshare = table.Column<string[]>(type: "text[]", nullable: true),
                    ScheduledTimes = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDepartures", x => x.FlightNumber);
                    table.ForeignKey(
                        name: "FK_ScheduledDepartures_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledDepartures_Destinations_DestinationFromIATACode",
                        column: x => x.DestinationFromIATACode,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                    table.ForeignKey(
                        name: "FK_ScheduledDepartures_Destinations_DestinationToIATACode",
                        column: x => x.DestinationToIATACode,
                        principalTable: "Destinations",
                        principalColumn: "IATAAirportCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledArrivals_AirlineId",
                table: "ScheduledArrivals",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledArrivals_DestinationFromIATACode",
                table: "ScheduledArrivals",
                column: "DestinationFromIATACode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledArrivals_DestinationToIATACode",
                table: "ScheduledArrivals",
                column: "DestinationToIATACode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDepartures_AirlineId",
                table: "ScheduledDepartures",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDepartures_DestinationFromIATACode",
                table: "ScheduledDepartures",
                column: "DestinationFromIATACode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDepartures_DestinationToIATACode",
                table: "ScheduledDepartures",
                column: "DestinationToIATACode");
        }
    }
}
