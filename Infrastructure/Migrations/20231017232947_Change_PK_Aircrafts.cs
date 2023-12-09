using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_PK_Aircrafts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aircrafts",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Aircrafts");

            migrationBuilder.AlterColumn<string>(
                name: "AircraftId",
                table: "Flight",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationCode",
                table: "Aircrafts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aircrafts",
                table: "Aircrafts",
                column: "RegistrationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "RegistrationCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aircrafts",
                table: "Aircrafts");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftId",
                table: "Flight",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationCode",
                table: "Aircrafts",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aircrafts",
                table: "Aircrafts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Aircrafts_AircraftId",
                table: "Flight",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
