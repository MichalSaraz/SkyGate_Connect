using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removing_Table_FlightClassSpecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightClassSpecifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightClassSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BassinetSeats = table.Column<string>(type: "jsonb", nullable: true),
                    ExitRowSeats = table.Column<string>(type: "jsonb", nullable: true),
                    FlightClass = table.Column<string>(type: "text", nullable: false),
                    NotExistingSeats = table.Column<string>(type: "jsonb", nullable: true),
                    RowRange = table.Column<string>(type: "jsonb", nullable: true),
                    SeatPositionsAvailable = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightClassSpecifications", x => x.Id);
                });
        }
    }
}
