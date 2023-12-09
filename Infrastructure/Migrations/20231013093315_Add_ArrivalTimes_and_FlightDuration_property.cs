using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ArrivalTimes_and_FlightDuration_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduledTimes",
                table: "ScheduledFlights",
                newName: "FlightDuration");

            migrationBuilder.AddColumn<string>(
                name: "ArrivalTimes",
                table: "ScheduledFlights",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureTimes",
                table: "ScheduledFlights",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTimes",
                table: "ScheduledFlights");

            migrationBuilder.DropColumn(
                name: "DepartureTimes",
                table: "ScheduledFlights");

            migrationBuilder.RenameColumn(
                name: "FlightDuration",
                table: "ScheduledFlights",
                newName: "ScheduledTimes");
        }
    }
}
