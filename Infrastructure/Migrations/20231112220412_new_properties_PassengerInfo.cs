using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new_properties_PassengerInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_PassengerInfos_PassengerId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Baggage_PassengerInfos_PassengerId",
                table: "Baggage");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftRegistrationCode",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerComment_PassengerInfos_PassengerId",
                table: "PassengerComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_PassengerInfos_PassengerId",
                table: "PassengerFlight");            

            migrationBuilder.DropIndex(
                name: "IX_Flights_AircraftRegistrationCode",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "AcceptanceStatus",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "BoardingSequenceNumber",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "BoardingZone",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "AircraftRegistrationCode",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Aircrafts");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "PassengerInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaggageAllowance",
                table: "PassengerInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PriorityBoarding",
                table: "PassengerInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    BoardingSequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    BoardingZone = table.Column<string>(type: "text", nullable: false),
                    AcceptanceStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passengers_PassengerInfos_Id",
                        column: x => x.Id,
                        principalTable: "PassengerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Passengers_PassengerId",
                table: "APISData",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baggage_Passengers_PassengerId",
                table: "Baggage",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerComment_Passengers_PassengerId",
                table: "PassengerComment",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Passengers_PassengerId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Baggage_Passengers_PassengerId",
                table: "Baggage");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerComment_Passengers_PassengerId",
                table: "PassengerComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight");            

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "BaggageAllowance",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "PriorityBoarding",
                table: "PassengerInfos");

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceStatus",
                table: "PassengerInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardingSequenceNumber",
                table: "PassengerInfos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BoardingZone",
                table: "PassengerInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PassengerInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AircraftRegistrationCode",
                table: "Flights",
                type: "text",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_PassengerInfos_PassengerId",
                table: "APISData",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baggage_PassengerInfos_PassengerId",
                table: "Baggage",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftRegistrationCode",
                table: "Flights",
                column: "AircraftRegistrationCode",
                principalTable: "Aircrafts",
                principalColumn: "RegistrationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerComment_PassengerInfos_PassengerId",
                table: "PassengerComment",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_PassengerInfos_PassengerId",
                table: "PassengerFlight",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
