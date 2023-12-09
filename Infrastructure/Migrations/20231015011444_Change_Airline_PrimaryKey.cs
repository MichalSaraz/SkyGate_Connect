using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_Airline_PrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_Airlines_AirlineId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_Airlines_AirlineId",
                table: "SeatMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Airlines_AirlineId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNumbers_Airlines_AirlineId",
                table: "TagNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Airlines",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Airlines");

            migrationBuilder.AlterColumn<string>(
                name: "AirlineId",
                table: "TagNumbers",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AirlineId",
                table: "ScheduledFlights",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AirlineId",
                table: "SeatMaps",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AirlineId",
                table: "FrequentFlyers",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CarrierCode",
                table: "Airlines",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AirlineId",
                table: "Aircrafts",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Airlines",
                table: "Airlines",
                column: "CarrierCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_Airlines_AirlineId",
                table: "FrequentFlyers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_Airlines_AirlineId",
                table: "SeatMaps",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Airlines_AirlineId",
                table: "ScheduledFlights",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNumbers_Airlines_AirlineId",
                table: "TagNumbers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_Airlines_AirlineId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_Airlines_AirlineId",
                table: "SeatMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Airlines_AirlineId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_TagNumbers_Airlines_AirlineId",
                table: "TagNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Airlines",
                table: "Airlines");

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "TagNumbers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "ScheduledFlights",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "SeatMaps",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "FrequentFlyers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CarrierCode",
                table: "Airlines",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Airlines",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Airlines",
                table: "Airlines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_Airlines_AirlineId",
                table: "FrequentFlyers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_Airlines_AirlineId",
                table: "SeatMaps",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Airlines_AirlineId",
                table: "ScheduledFlights",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagNumbers_Airlines_AirlineId",
                table: "TagNumbers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
