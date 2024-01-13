using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Move_properties_to_PassengerFlight_and_IsEUCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptanceStatus",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "BoardingSequenceNumber",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "BoardingZone",
                table: "Passengers");

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceStatus",
                table: "PassengerFlight",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BoardingSequenceNumbers",
                table: "PassengerFlight",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BoardingZone",
                table: "PassengerFlight",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEUCountry",
                table: "Countries",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptanceStatus",
                table: "PassengerFlight");

            migrationBuilder.DropColumn(
                name: "BoardingSequenceNumbers",
                table: "PassengerFlight");

            migrationBuilder.DropColumn(
                name: "BoardingZone",
                table: "PassengerFlight");

            migrationBuilder.DropColumn(
                name: "IsEUCountry",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceStatus",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BoardingSequenceNumber",
                table: "Passengers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BoardingZone",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
