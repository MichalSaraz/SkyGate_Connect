using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_and_delete_some_indexes_part2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PassengerFlight_FlightId",
                table: "PassengerFlight",
                column: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerFlight_BasePassengerOrItemId",
                table: "PassengerFlight",
                newName: "IX_PassengerFlight_PassengerOrItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PassengerFlight_FlightId",
                table: "PassengerFlight");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerFlight_PassengerOrItemId",
                table: "PassengerFlight",
                newName: "IX_PassengerFlight_BasePassengerOrItemId");
        }
    }
}
