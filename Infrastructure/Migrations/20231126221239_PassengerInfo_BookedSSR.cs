using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PassengerInfo_BookedSSR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SSRList",
                table: "PassengerInfos");
            migrationBuilder.AddColumn<List<KeyValuePair<int, string>>>(
                name: "ReservedSeats",
                table: "PassengerInfos",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<List<KeyValuePair<int, string[]>>>(
                name: "BookedSSR",
                table: "PassengerInfos",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SSRList",
                table: "PassengerInfos",
                type: "jsonb",
                nullable: true);
            migrationBuilder.DropColumn(
                name: "ReservedSeats",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "BookedSSR",
                table: "PassengerInfos");
        }
    }
}
