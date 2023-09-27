using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_Aircraft_Create_ClassSeatMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Aircrafts_AircraftId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "ClassConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_Seats_AircraftId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "AircraftId",
                table: "Seats");

            migrationBuilder.AlterColumn<int>(
                name: "LeadingDigit",
                table: "TagNumbers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "Row",
                table: "Seats",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AddColumn<int>(
                name: "ClassSeatMapId",
                table: "Seats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightClass",
                table: "Seats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "Seats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JumpSeatsAvailable",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassSeatMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightClass = table.Column<string>(type: "text", nullable: false),
                    SeatPositionsAvailable = table.Column<string>(type: "text", nullable: false),
                    RowRange = table.Column<int[]>(type: "integer[]", nullable: true),
                    ExitRowSeats = table.Column<string[]>(type: "text[]", nullable: true),
                    BassinetSeats = table.Column<string[]>(type: "text[]", nullable: true),
                    NotExistingSeats = table.Column<string[]>(type: "text[]", nullable: true),
                    AircraftId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSeatMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSeatMap_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ClassSeatMapId",
                table: "Seats",
                column: "ClassSeatMapId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSeatMap_AircraftId",
                table: "ClassSeatMap",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_ClassSeatMap_ClassSeatMapId",
                table: "Seats",
                column: "ClassSeatMapId",
                principalTable: "ClassSeatMap",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_ClassSeatMap_ClassSeatMapId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "ClassSeatMap");

            migrationBuilder.DropIndex(
                name: "IX_Seats_ClassSeatMapId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "ClassSeatMapId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "FlightClass",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "JumpSeatsAvailable",
                table: "Aircrafts");

            migrationBuilder.AlterColumn<byte>(
                name: "LeadingDigit",
                table: "TagNumbers",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<byte>(
                name: "Row",
                table: "Seats",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AircraftId",
                table: "Seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    Class = table.Column<string>(type: "text", nullable: false),
                    NumberOfRows = table.Column<int>(type: "integer", nullable: false),
                    SeatPositionsAvailable = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassConfigurations_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_AircraftId",
                table: "Seats",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassConfigurations_AircraftId",
                table: "ClassConfigurations",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Aircrafts_AircraftId",
                table: "Seats",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
