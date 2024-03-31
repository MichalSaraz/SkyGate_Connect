using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_PK_PassengerBookingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPassengerId",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassengerInfo",
                table: "PassengerBookingDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PassengerBookingDetails");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "PassengerBookingDetails",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassengerBookingDetails",
                table: "PassengerBookingDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
               name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPassengerId",
               table: "PassengerBookingDetails",
               column: "AssociatedPassengerId",
               principalTable: "PassengerBookingDetails",
               principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                principalTable: "PassengerBookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
               name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPassengerId",
               table: "PassengerBookingDetails",
               column: "AssociatedPassengerId",
               principalTable: "PassengerBookingDetails",
               principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                principalTable: "PassengerBookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FrequentFlyerCards",
                newName: "NewId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FrequentFlyerCards",
                type: "uuid",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassengerInfo",
                table: "PassengerBookingDetails",
                column: "Id");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPassengerId",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers");
        }
    }
}
