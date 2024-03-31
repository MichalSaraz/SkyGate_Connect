using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_BasePassengerOrItem_and_derived_classes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_Airlines_AirlineId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_PassengerInfo_PassengerInfoId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerInfo_Id",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Passengers_PassengerId",
                table: "Seats");

            // 1) Smazání cizího klíče
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerInfo_BookingReferences_PNRId",
                table: "PassengerInfo");

            // 2) Smazání indexu
            migrationBuilder.DropIndex(
                name: "IX_PassengerInfo_PNRId",
                table: "PassengerInfo");

            // 3) Přejmenování tabulky
            migrationBuilder.RenameTable(
                name: "PassengerInfo",
                newName: "PassengerBookingDetails");

            // 4) Úprava sloupce na nullable: true
            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "PassengerBookingDetails",
                type: "integer",
                nullable: true,
                oldNullable: false);

            // 5) Smazání sloupce
            migrationBuilder.DropColumn(
                name: "FrequentFlyerId",
                table: "PassengerBookingDetails");

            


            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequentFlyers",
                table: "FrequentFlyers");

            migrationBuilder.RenameTable(
                name: "FrequentFlyers",
                newName: "FrequentFlyerCards");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "Seats",
                newName: "PassengerOrItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_PassengerId",
                table: "Seats",
                newName: "IX_Seats_PassengerOrItemId");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "PassengerFlight",
                newName: "PassengerOrItemId");

            migrationBuilder.RenameColumn(
                name: "IsEUCountry",
                table: "Countries",
                newName: "IsEEACountry");

            migrationBuilder.RenameColumn(
                name: "PassengerInfoId",
                table: "FrequentFlyerCards",
                newName: "PassengerId");

            migrationBuilder.RenameColumn(
                name: "CardholderName",
                table: "FrequentFlyerCards",
                newName: "CardholderLastName");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyers_PassengerInfoId",
                table: "FrequentFlyerCards",
                newName: "IX_FrequentFlyerCards_PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyers_CardNumber",
                table: "FrequentFlyerCards",
                newName: "IX_FrequentFlyerCards_CardNumber");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyers_AirlineId",
                table: "FrequentFlyerCards",
                newName: "IX_FrequentFlyerCards_AirlineId");

            // 6) Přidání sloupce
            migrationBuilder.AddColumn<string>(
                name: "FrequentFlyerCardNumber",
                table: "PassengerBookingDetails",
                type: "text",
                nullable: true);

            // 7) Přidání sloupce
            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "PassengerBookingDetails",
                type: "uuid",
                nullable: true);


            // 8) Přidání sloupce
            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedPassengerId",
                table: "PassengerBookingDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedPassengerId",
                table: "Passengers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaggageAllowance",
                table: "Passengers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingDetailsId",
                table: "Passengers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FrequentFlyerCardId",
                table: "Passengers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "InfantId",
                table: "Passengers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassengerOrItemType",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PriorityBoarding",
                table: "Passengers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Passengers",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PassengerId",
                table: "APISData",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "CardholderFirstName",
                table: "FrequentFlyerCards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "FrequentFlyerCards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards",
                column: "Id");

            

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_AssociatedPassengerId",
                table: "Passengers",
                column: "AssociatedPassengerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookingDetails_AssociatedPassengerId",
                table: "PassengerBookingDetails",
                column: "AssociatedPassengerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookingDetails_PNRId",
                table: "PassengerBookingDetails",
                column: "PNRId");

            //new
            migrationBuilder.AddForeignKey(
                name: "FK_PassengerBookingDetails_BookingReferences_PNRId",
                table: "PassengerBookingDetails",
                column: "PNRId",
                principalTable: "BookingReferences",
                principalColumn: "PNR",
                onDelete: ReferentialAction.Cascade);

            //new
            migrationBuilder.AddForeignKey(
               name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPassengerId",
               table: "PassengerBookingDetails",
               column: "AssociatedPassengerId",
               principalTable: "PassengerBookingDetails",
               principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyerCards_Airlines_AirlineId",
                table: "FrequentFlyerCards",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyerCards_Passengers_PassengerId",
                table: "FrequentFlyerCards",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerOrItemId",
                table: "PassengerFlight",
                column: "PassengerOrItemId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                principalTable: "PassengerBookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_AssociatedPassengerId",
                table: "Passengers",
                column: "AssociatedPassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Passengers_PassengerOrItemId",
                table: "Seats",
                column: "PassengerOrItemId",
                principalTable: "Passengers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_BookingReferences_PNRId",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPassengerId",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyerCards_Airlines_AirlineId",
                table: "FrequentFlyerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyerCards_Passengers_PassengerId",
                table: "FrequentFlyerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerOrItemId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_AssociatedPassengerId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Passengers_PassengerOrItemId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_AssociatedPassengerId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_BookingDetailsId",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards");

            migrationBuilder.DropColumn(
                name: "AssociatedPassengerId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "BaggageAllowance",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "BookingDetailsId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "FrequentFlyerCardId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "InfantId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "PassengerOrItemType",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "PriorityBoarding",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "CardholderFirstName",
                table: "FrequentFlyerCards");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "FrequentFlyerCards");

            // 8) Odstranění sloupce
            migrationBuilder.DropColumn(
                name: "AssociatedPassengerId",
                table: "PassengerBookingDetails");

            // 7) Odstranění sloupce
            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "PassengerBookingDetails");

            // 6) Odstranění sloupce
            migrationBuilder.DropColumn(
                name: "FrequentFlyerCardNumber",
                table: "PassengerBookingDetails");

            migrationBuilder.RenameTable(
                name: "FrequentFlyerCards",
                newName: "FrequentFlyers");

            migrationBuilder.RenameColumn(
                name: "PassengerOrItemId",
                table: "Seats",
                newName: "PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_PassengerOrItemId",
                table: "Seats",
                newName: "IX_Seats_PassengerId");

            migrationBuilder.RenameColumn(
                name: "PassengerOrItemId",
                table: "PassengerFlight",
                newName: "PassengerId");

            migrationBuilder.RenameColumn(
                name: "IsEEACountry",
                table: "Countries",
                newName: "IsEUCountry");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "FrequentFlyers",
                newName: "PassengerInfoId");

            migrationBuilder.RenameColumn(
                name: "CardholderLastName",
                table: "FrequentFlyers",
                newName: "CardholderName");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyerCards_PassengerId",
                table: "FrequentFlyers",
                newName: "IX_FrequentFlyers_PassengerInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyerCards_CardNumber",
                table: "FrequentFlyers",
                newName: "IX_FrequentFlyers_CardNumber");

            migrationBuilder.RenameIndex(
                name: "IX_FrequentFlyerCards_AirlineId",
                table: "FrequentFlyers",
                newName: "IX_FrequentFlyers_AirlineId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassengerId",
                table: "APISData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequentFlyers",
                table: "FrequentFlyers",
                column: "Id");           

            // 5) Přidání sloupce
            migrationBuilder.AddColumn<Guid>(
                name: "FrequentFlyerId",
                table: "PassengerBookingDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // 4) Zpětná úprava sloupce na nullable: false
            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "PassengerBookingDetails",
                type: "integer",
                nullable: false,
                oldNullable: true);

            // 3) Zpětné přejmenování tabulky
            migrationBuilder.RenameTable(
                name: "PassengerBookingDetails",
                newName: "PassengerInfo");


            migrationBuilder.CreateIndex(
                name: "IX_PassengerInfo_PNRId",
                table: "PassengerInfo",
                column: "PNRId");

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_Airlines_AirlineId",
                table: "FrequentFlyers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "CarrierCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_PassengerInfo_PassengerInfoId",
                table: "FrequentFlyers",
                column: "PassengerInfoId",
                principalTable: "PassengerInfo",
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
                name: "FK_PassengerInfo_BookingReferences_PNRId",
                table: "PassengerInfo",
                column: "PNRId",
                principalTable: "BookingReferences",
                principalColumn: "PNR",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerInfo_Id",
                table: "Passengers",
                column: "Id",
                principalTable: "PassengerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Passengers_PassengerId",
                table: "Seats",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");
        }
    }
}
