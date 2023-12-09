using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class minor_changes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AircraftRegistrationCode",
                table: "Flights",
                newName: "AircraftId");

            migrationBuilder.DropForeignKey(
               name: "FK_Flights_Aircrafts_AircraftRegistrationCode",
               table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AircraftRegistrationCode",
                table: "Flights");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftId",
                table: "Flights",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "RegistrationCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AircraftId",
                table: "Flights",
                newName: "AircraftRegistrationCode");

            migrationBuilder.AddForeignKey(
               name: "FK_Flights_Aircrafts_AircraftRegistrationCode",
               table: "Flights",
               column: "AircraftRegistrationCode",
               principalTable: "Aircrafts",
               principalColumn: "RegistrationCode");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftRegistrationCode",
                table: "Flights",
                column: "AircraftRegistrationCode");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
               name: "FK_Flights_Aircrafts_AircraftId",
               table: "Flights");
        }
    }
}
