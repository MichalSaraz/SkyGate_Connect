using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_SeatMaps_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                table: "SeatMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_Airlines_AirlineId",
                table: "SeatMaps");

            migrationBuilder.DropIndex(
                name: "IX_SeatMaps_AircraftTypeId",
                table: "SeatMaps");

            migrationBuilder.DropIndex(
                name: "IX_SeatMaps_AirlineId",
                table: "SeatMaps");

            migrationBuilder.DropColumn(
                name: "AircraftTypeId",
                table: "SeatMaps");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "SeatMaps");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "SeatMaps",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "SeatMapId",
                table: "Aircrafts",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_SeatMaps_SeatMapId",
                table: "Aircrafts",
                column: "SeatMapId",
                principalTable: "SeatMaps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_SeatMaps_SeatMapId",
                table: "Aircrafts");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SeatMaps",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "AircraftTypeId",
                table: "SeatMaps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AirlineId",
                table: "SeatMaps",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SeatMapId",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatMaps_AircraftTypeId",
                table: "SeatMaps",
                column: "AircraftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatMaps_AirlineId",
                table: "SeatMaps",
                column: "AirlineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_SeatMaps_SeatMapId",
                table: "Aircrafts",
                column: "SeatMapId",
                principalTable: "SeatMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                table: "SeatMaps",
                column: "AircraftTypeId",
                principalTable: "AircraftTypes",
                principalColumn: "AircraftTypeIATACode");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_Airlines_AirlineId",
                table: "SeatMaps",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode");
        }
    }
}
