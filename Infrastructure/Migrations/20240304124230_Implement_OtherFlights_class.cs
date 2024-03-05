using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Implement_OtherFlights_class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationToId",
                table: "ScheduledFlights",
                newName: "DestinationTo");

            migrationBuilder.RenameColumn(
                name: "DestinationFromId",
                table: "ScheduledFlights",
                newName: "DestinationFrom");

            migrationBuilder.RenameColumn(
                name: "AirlineId",
                table: "ScheduledFlights",
                newName: "Airline");

            migrationBuilder.AlterColumn<string>(
                name: "FlightStatus",
                table: "Flights",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalDateTime",
                table: "Flights",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "FlightType",
                table: "Flights",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherFlightFltNumber",
                table: "Flights",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightType",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "OtherFlightFltNumber",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "DestinationTo",
                table: "ScheduledFlights",
                newName: "DestinationToId");

            migrationBuilder.RenameColumn(
                name: "DestinationFrom",
                table: "ScheduledFlights",
                newName: "DestinationFromId");

            migrationBuilder.RenameColumn(
                name: "Airline",
                table: "ScheduledFlights",
                newName: "AirlineId");

            migrationBuilder.AlterColumn<string>(
                name: "FlightStatus",
                table: "Flights",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalDateTime",
                table: "Flights",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
