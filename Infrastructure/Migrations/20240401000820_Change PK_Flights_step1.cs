using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePK_Flights_step1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "FlightId2",
                table: "SpecialServiceRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FlightId2",
                table: "Seats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FlightId2",
                table: "PassengerFlight",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "Flights",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FlightId2",
                table: "FlightComment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FlightId2",
                table: "FlightBaggage",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightId2",
                table: "SpecialServiceRequests");

            migrationBuilder.DropColumn(
                name: "FlightId2",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "FlightId2",
                table: "PassengerFlight");

            migrationBuilder.DropColumn(
                name: "FlightId2",
                table: "FlightComment");

            migrationBuilder.DropColumn(
                name: "FlightId2",
                table: "FlightBaggage");

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
    }
}
