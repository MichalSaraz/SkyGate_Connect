using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Major_Changes_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Passengers_PassengerId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Baggage_Passengers_PassengerId",
                table: "Baggage");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_Passengers_PassengerId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerComment_Passengers_PassengerId",
                table: "PassengerComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_BookingReferences_PNRId",
                table: "Passengers");            

            migrationBuilder.DropForeignKey(
                name: "FK_SSRCodes_SpecialServiceRequest_SpecialServiceRequestId",
                table: "SSRCodes");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "PassengerSpecialServiceRequest");

            migrationBuilder.DropTable(
                name: "SpecialServiceRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SSRCodes",
                table: "SSRCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingReferences",
                table: "BookingReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passengers",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "SpecialServiceRequestId",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookingReferences");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "APISData");

            migrationBuilder.RenameTable(
                name: "Passengers",
                newName: "PassengerInfos");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "FrequentFlyers",
                newName: "PassengerInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyers_PassengerId",
                table: "FrequentFlyers",
                newName: "IX_FrequentFlyers_PassengerInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Passengers_PNRId",
                table: "PassengerInfos",
                newName: "IX_PassengerInfos_PNRId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SSRCodes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "SSRCodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FreeText",
                table: "SSRCodes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeTextMandatory",
                table: "SSRCodes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengerInfoId",
                table: "SSRCodes",
                type: "integer",
                nullable: true);            

            migrationBuilder.AlterColumn<string[]>(
                name: "AircraftRegistrationPrefix",
                table: "Countries",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightItinerary",
                table: "BookingReferences",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "APISData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PNRId",
                table: "PassengerInfos",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "BoardingZone",
                table: "PassengerInfos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "BoardingSequenceNumber",
                table: "PassengerInfos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AcceptanceStatus",
                table: "PassengerInfos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PassengerInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "ReservedSeats",
                table: "PassengerInfos",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SSRCodes",
                table: "SSRCodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingReferences",
                table: "BookingReferences",
                column: "PNR");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassengerInfos",
                table: "PassengerInfos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SSRCodes_PassengerInfoId",
                table: "SSRCodes",
                column: "PassengerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_PassengerInfos_PassengerId",
                table: "APISData",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baggage_PassengerInfos_PassengerId",
                table: "Baggage",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
                table: "FrequentFlyers",
                column: "PassengerInfoId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerComment_PassengerInfos_PassengerId",
                table: "PassengerComment",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_PassengerInfos_PassengerId",
                table: "PassengerFlight",
                column: "PassengerId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerInfos_BookingReferences_PNRId",
                table: "PassengerInfos",
                column: "PNRId",
                principalTable: "BookingReferences",
                principalColumn: "PNR",
                onDelete: ReferentialAction.Cascade);            

            migrationBuilder.AddForeignKey(
                name: "FK_SSRCodes_PassengerInfos_PassengerInfoId",
                table: "SSRCodes",
                column: "PassengerInfoId",
                principalTable: "PassengerInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_PassengerInfos_PassengerId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Baggage_PassengerInfos_PassengerId",
                table: "Baggage");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerComment_PassengerInfos_PassengerId",
                table: "PassengerComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_PassengerInfos_PassengerId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerInfos_BookingReferences_PNRId",
                table: "PassengerInfos");           

            migrationBuilder.DropForeignKey(
                name: "FK_SSRCodes_PassengerInfos_PassengerInfoId",
                table: "SSRCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SSRCodes",
                table: "SSRCodes");

            migrationBuilder.DropIndex(
                name: "IX_SSRCodes_PassengerInfoId",
                table: "SSRCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingReferences",
                table: "BookingReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassengerInfos",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "FreeText",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "IsFreeTextMandatory",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "PassengerInfoId",
                table: "SSRCodes");

            migrationBuilder.DropColumn(
                name: "FlightItinerary",
                table: "BookingReferences");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "APISData");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PassengerInfos");

            migrationBuilder.DropColumn(
                name: "ReservedSeats",
                table: "PassengerInfos");

            migrationBuilder.RenameTable(
                name: "PassengerInfos",
                newName: "Passengers");

            migrationBuilder.RenameColumn(
                name: "PassengerInfoId",
                table: "FrequentFlyers",
                newName: "PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyers_PassengerInfoId",
                table: "FrequentFlyers",
                newName: "IX_FrequentFlyers_PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerInfos_PNRId",
                table: "Passengers",
                newName: "IX_Passengers_PNRId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SSRCodes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "SpecialServiceRequestId",
                table: "SSRCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);               

            migrationBuilder.AlterColumn<string>(
                name: "AircraftRegistrationPrefix",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookingReferences",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "APISData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PNRId",
                table: "Passengers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BoardingZone",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoardingSequenceNumber",
                table: "Passengers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AcceptanceStatus",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SSRCodes",
                table: "SSRCodes",
                column: "SpecialServiceRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingReferences",
                table: "BookingReferences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passengers",
                table: "Passengers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    APISDataId = table.Column<int>(type: "integer", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.APISDataId);
                    table.ForeignKey(
                        name: "FK_DocumentTypes_APISData_APISDataId",
                        column: x => x.APISDataId,
                        principalTable: "APISData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialServiceRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FreeText = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    IsFreeTextMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialServiceRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassengerSpecialServiceRequest",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    SpecialServiceRequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerSpecialServiceRequest", x => new { x.PassengerId, x.SpecialServiceRequestId });
                    table.ForeignKey(
                        name: "FK_PassengerSpecialServiceRequest_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerSpecialServiceRequest_SpecialServiceRequest_Specia~",
                        column: x => x.SpecialServiceRequestId,
                        principalTable: "SpecialServiceRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassengerSpecialServiceRequest_SpecialServiceRequestId",
                table: "PassengerSpecialServiceRequest",
                column: "SpecialServiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Passengers_PassengerId",
                table: "APISData",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baggage_Passengers_PassengerId",
                table: "Baggage",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_Passengers_PassengerId",
                table: "FrequentFlyers",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerComment_Passengers_PassengerId",
                table: "PassengerComment",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_BookingReferences_PNRId",
                table: "Passengers",
                column: "PNRId",
                principalTable: "BookingReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);            

            migrationBuilder.AddForeignKey(
                name: "FK_SSRCodes_SpecialServiceRequest_SpecialServiceRequestId",
                table: "SSRCodes",
                column: "SpecialServiceRequestId",
                principalTable: "SpecialServiceRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
