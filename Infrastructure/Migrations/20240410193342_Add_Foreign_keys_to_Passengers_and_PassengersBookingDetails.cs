using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Foreign_keys_to_Passengers_and_PassengersBookingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPa",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingDetailsId",
                table: "Passengers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookingDetails_PassengerId",
                table: "PassengerBookingDetails",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_InfantId",
                table: "Passengers",
                column: "InfantId");

            migrationBuilder.AddForeignKey(
               name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPa",
               table: "PassengerBookingDetails",
               column: "AssociatedPassengerBookingDetailsId",
               principalTable: "PassengerBookingDetails",
               principalColumn: "Id",
               onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                principalTable: "PassengerBookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                table: "Passengers",
                column: "AssociatedAdultPassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerBookingDetails_Passengers_PassengerId",
                table: "PassengerBookingDetails",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_InfantId",
                table: "Passengers",
                column: "InfantId",
                principalTable: "Passengers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPa",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerBookingDetails_Passengers_PassengerId",
                table: "PassengerBookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_InfantId",
                table: "Passengers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingDetailsId",
                table: "Passengers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_PassengerBookingDetails_PassengerId",
                table: "PassengerBookingDetails");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_InfantId",
                table: "Passengers");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerBookingDetails_BookingDetailsId",
                table: "Passengers",
                column: "BookingDetailsId",
                principalTable: "PassengerBookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerBookingDetails_PassengerBookingDetails_AssociatedPa",
                table: "PassengerBookingDetails",
                column: "AssociatedPassengerBookingDetailsId",
                principalTable: "PassengerBookingDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                table: "Passengers",
                column: "AssociatedAdultPassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}