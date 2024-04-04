using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_PK_Flights_step2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "FlightComment");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "PassengerFlight");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Flights",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FlightId2",
                table: "FlightBaggage",
                newName: "FlightId");

            migrationBuilder.RenameColumn(
                name: "FlightId2",
                table: "FlightComment",
                newName: "FlightId");

            migrationBuilder.RenameColumn(
                name: "FlightId2",
                table: "PassengerFlight",
                newName: "FlightId");

            migrationBuilder.RenameColumn(
                name: "FlightId2",
                table: "Seats",
                newName: "FlightId");

            migrationBuilder.RenameColumn(
                name: "FlightId2",
                table: "SpecialServiceRequests",
                newName: "FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flights",
                table: "Flights",
                column: "Id");

            migrationBuilder.AddForeignKey(
               name: "FK_SpecialServiceRequests_Flights_FlightId",
               table: "SpecialServiceRequests",
               column: "FlightId",
               principalTable: "Flights",
               principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Flights_FlightId",
                table: "Seats",
                column: "FlightId",
                principalTable: "Flights",
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
                name: "FK_FlightComment_Flights_FlightId",
                table: "FlightComment",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightBaggage_Flights_FlightId",
                table: "FlightBaggage",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_Flights_FlightId",
                table: "SpecialServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Flights_FlightId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Flights_FlightId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
               name: "FK_FlightComment_Flights_FlightId",
               table: "FlightComment");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Flights_FlightId",
                table: "FlightBaggage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");
            
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Flights",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "FlightBaggage",
                newName: "FlightId2");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "FlightComment",
                newName: "FlightId2");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "PassengerFlight",
                newName: "FlightId2");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "Seats",
                newName: "FlightId2");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "SpecialServiceRequests",
                newName: "FlightId2");

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "FlightBaggage",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "FlightComment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "PassengerFlight",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "SpecialServiceRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flights",
                table: "Flights",
                column: "Id");
        }
    }
}
