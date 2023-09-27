using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Major_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_SeatMaps_SeatMapId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatMaps_SeatMapId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "SeatMaps");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_AircraftRegistration",
                table: "Aircrafts");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_SeatMapId",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "BoardingZone",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "AircraftRegistration",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "AircraftType",
                table: "Aircrafts");

            migrationBuilder.RenameColumn(
                name: "SeatMapId",
                table: "Seats",
                newName: "AircraftId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_SeatMapId",
                table: "Seats",
                newName: "IX_Seats_AircraftId");

            migrationBuilder.RenameColumn(
                name: "SeatMapId",
                table: "Aircrafts",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "AircraftTypeIATACode",
                table: "Aircrafts",
                newName: "RegistrationCode");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Seats",
                type: "text",
                nullable: false,
                oldClrType: typeof(char),
                oldType: "character(1)");

            migrationBuilder.AddColumn<string>(
                name: "BoardingZone",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AircraftRegistrationPrefix",
                table: "Countries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AircraftTypeId",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AircraftTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelName = table.Column<string>(type: "text", nullable: true),
                    AircraftTypeIATACode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AircraftTypeId",
                table: "Aircrafts",
                column: "AircraftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AirlineId",
                table: "Aircrafts",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_CountryId",
                table: "Aircrafts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_RegistrationCode",
                table: "Aircrafts",
                column: "RegistrationCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                table: "Aircrafts",
                column: "AircraftTypeId",
                principalTable: "AircraftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_Countries_CountryId",
                table: "Aircrafts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Aircrafts_AircraftId",
                table: "Seats",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Countries_CountryId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Aircrafts_AircraftId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "AircraftTypes");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_AircraftTypeId",
                table: "Aircrafts");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_CountryId",
                table: "Aircrafts");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_RegistrationCode",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "BoardingZone",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "AircraftRegistrationPrefix",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "AircraftTypeId",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "Aircrafts");

            migrationBuilder.RenameColumn(
                name: "AircraftId",
                table: "Seats",
                newName: "SeatMapId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_AircraftId",
                table: "Seats",
                newName: "IX_Seats_SeatMapId");

            migrationBuilder.RenameColumn(
                name: "RegistrationCode",
                table: "Aircrafts",
                newName: "AircraftTypeIATACode");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Aircrafts",
                newName: "SeatMapId");

            migrationBuilder.AlterColumn<char>(
                name: "Position",
                table: "Seats",
                type: "character(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BoardingZone",
                table: "Seats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AircraftRegistration",
                table: "Aircrafts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AircraftType",
                table: "Aircrafts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeatMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatMaps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AircraftRegistration",
                table: "Aircrafts",
                column: "AircraftRegistration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_SeatMapId",
                table: "Aircrafts",
                column: "SeatMapId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_SeatMaps_SeatMapId",
                table: "Aircrafts",
                column: "SeatMapId",
                principalTable: "SeatMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_SeatMaps_SeatMapId",
                table: "Seats",
                column: "SeatMapId",
                principalTable: "SeatMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
