using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Move_BaggageType_prop_to_FlightBaggage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaggageType",
                table: "Baggage");

            migrationBuilder.AddColumn<string>(
                name: "BaggageType",
                table: "FlightBaggage",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagType",
                table: "Baggage",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaggageType",
                table: "FlightBaggage");

            migrationBuilder.DropColumn(
                name: "TagType",
                table: "Baggage");

            migrationBuilder.AddColumn<string>(
                name: "BaggageType",
                table: "Baggage",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
