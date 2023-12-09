using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_Destination_Primary_Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_ArrivalAirportId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DepartureAirportId",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_ArrivalAirportId",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_DepartureAirportId",
                table: "ScheduledFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportId",
                table: "ScheduledFlights");

            migrationBuilder.DropColumn(
                name: "DepartureAirportId",
                table: "ScheduledFlights");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Destinations");

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAirportIATACode",
                table: "ScheduledFlights",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureAirportIATACode",
                table: "ScheduledFlights",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IATAAirportCode",
                table: "Destinations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations",
                column: "IATAAirportCode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_ArrivalAirportIATACode",
                table: "ScheduledFlights",
                column: "ArrivalAirportIATACode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DepartureAirportIATACode",
                table: "ScheduledFlights",
                column: "DepartureAirportIATACode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_ArrivalAirportIATACode",
                table: "ScheduledFlights",
                column: "ArrivalAirportIATACode",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DepartureAirportIATACode",
                table: "ScheduledFlights",
                column: "DepartureAirportIATACode",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_ArrivalAirportIATACode",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DepartureAirportIATACode",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_ArrivalAirportIATACode",
                table: "ScheduledFlights");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledFlights_DepartureAirportIATACode",
                table: "ScheduledFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportIATACode",
                table: "ScheduledFlights");

            migrationBuilder.DropColumn(
                name: "DepartureAirportIATACode",
                table: "ScheduledFlights");

            migrationBuilder.AddColumn<int>(
                name: "ArrivalAirportId",
                table: "ScheduledFlights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartureAirportId",
                table: "ScheduledFlights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IATAAirportCode",
                table: "Destinations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Destinations",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_ArrivalAirportId",
                table: "ScheduledFlights",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlights_DepartureAirportId",
                table: "ScheduledFlights",
                column: "DepartureAirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_ArrivalAirportId",
                table: "ScheduledFlights",
                column: "ArrivalAirportId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DepartureAirportId",
                table: "ScheduledFlights",
                column: "DepartureAirportId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
