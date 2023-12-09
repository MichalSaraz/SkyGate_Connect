using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_PK_AircraftType_Country : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Countries_CountryId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Airlines_Countries_CountryId",
                table: "Airlines");

            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Countries_IssueCountryId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Countries_NationalityId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Countries_CountryId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                table: "SeatMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationFromId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationToId",
                table: "ScheduledFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AircraftTypes",
                table: "AircraftTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AircraftTypes");                 

            migrationBuilder.AlterColumn<string>(
                name: "AircraftTypeId",
                table: "SeatMaps",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Destinations",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Country2LetterCode",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalityId",
                table: "APISData",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IssueCountryId",
                table: "APISData",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Airlines",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AircraftTypeIATACode",
                table: "AircraftTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Aircrafts",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AircraftTypeId",
                table: "Aircrafts",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Country2LetterCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AircraftTypes",
                table: "AircraftTypes",
                column: "AircraftTypeIATACode");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                table: "Aircrafts",
                column: "AircraftTypeId",
                principalTable: "AircraftTypes",
                principalColumn: "AircraftTypeIATACode");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_Countries_CountryId",
                table: "Aircrafts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Airlines_Countries_CountryId",
                table: "Airlines",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Countries_IssueCountryId",
                table: "APISData",
                column: "IssueCountryId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Countries_NationalityId",
                table: "APISData",
                column: "NationalityId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Countries_CountryId",
                table: "Destinations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Country2LetterCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                table: "SeatMaps",
                column: "AircraftTypeId",
                principalTable: "AircraftTypes",
                principalColumn: "AircraftTypeIATACode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationFromId",
                table: "ScheduledFlights",
                column: "DestinationFromId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationToId",
                table: "ScheduledFlights",
                column: "DestinationToId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Countries_CountryId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Airlines_Countries_CountryId",
                table: "Airlines");

            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Countries_IssueCountryId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Countries_NationalityId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Countries_CountryId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                table: "SeatMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationFromId",
                table: "ScheduledFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationToId",
                table: "ScheduledFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AircraftTypes",
                table: "AircraftTypes");

            migrationBuilder.RenameColumn(
                name: "DestinationToId",
                table: "ScheduledFlights",
                newName: "DestinationToIATACode");

            migrationBuilder.RenameColumn(
                name: "DestinationFromId",
                table: "ScheduledFlights",
                newName: "DestinationFromIATACode");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduledFlights_DestinationToId",
                table: "ScheduledFlights",
                newName: "IX_ScheduledFlights_DestinationToIATACode");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduledFlights_DestinationFromId",
                table: "ScheduledFlights",
                newName: "IX_ScheduledFlights_DestinationFromIATACode");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftTypeId",
                table: "SeatMaps",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Destinations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country2LetterCode",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Countries",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "APISData",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IssueCountryId",
                table: "APISData",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Airlines",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AircraftTypeIATACode",
                table: "AircraftTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AircraftTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AircraftTypeId",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AircraftTypes",
                table: "AircraftTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_AircraftTypes_AircraftTypeId",
                table: "Aircrafts",
                column: "AircraftTypeId",
                principalTable: "AircraftTypes",
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
                name: "FK_Airlines_Countries_CountryId",
                table: "Airlines",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Countries_IssueCountryId",
                table: "APISData",
                column: "IssueCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Countries_NationalityId",
                table: "APISData",
                column: "NationalityId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Countries_CountryId",
                table: "Destinations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_AircraftTypes_AircraftTypeId",
                table: "SeatMaps",
                column: "AircraftTypeId",
                principalTable: "AircraftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationFromIATACode",
                table: "ScheduledFlights",
                column: "DestinationFromIATACode",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledFlights_Destinations_DestinationToIATACode",
                table: "ScheduledFlights",
                column: "DestinationToIATACode",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");
        }
    }
}
