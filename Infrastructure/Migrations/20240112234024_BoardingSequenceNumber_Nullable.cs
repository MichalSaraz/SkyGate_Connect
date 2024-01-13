using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BoardingSequenceNumber_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardingSequenceNumbers",
                table: "PassengerFlight");

            migrationBuilder.AddColumn<int>(
                name: "BoardingSequenceNumber",
                table: "PassengerFlight",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardingSequenceNumber",
                table: "PassengerFlight");

            migrationBuilder.AddColumn<int>(
                name: "BoardingSequenceNumbers",
                table: "PassengerFlight",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
