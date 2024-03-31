using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rename_some_props_in_PassengerContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_AssociatedPassengerId",
                table: "Passengers");

            migrationBuilder.RenameColumn(
                name: "AssociatedPassengerId",
                table: "Passengers",
                newName: "AssociatedAdultPassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_Passengers_AssociatedPassengerId",
                table: "Passengers",
                newName: "IX_Passengers_AssociatedAdultPassengerId");

            migrationBuilder.RenameColumn(
                name: "AssociatedPassengerId",
                table: "PassengerBookingDetails",
                newName: "AssociatedPassengerBookingDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerBookingDetails_AssociatedPassengerId",
                table: "PassengerBookingDetails",
                newName: "IX_PassengerBookingDetails_AssociatedPassengerBookingDetailsId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassengerId",
                table: "PassengerBookingDetails",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingDetailsId",
                table: "Passengers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                table: "Passengers",
                column: "AssociatedAdultPassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_AssociatedAdultPassengerId",
                table: "Passengers");

            migrationBuilder.RenameColumn(
                name: "AssociatedAdultPassengerId",
                table: "Passengers",
                newName: "AssociatedPassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_Passengers_AssociatedAdultPassengerId",
                table: "Passengers",
                newName: "IX_Passengers_AssociatedPassengerId");

            migrationBuilder.RenameColumn(
                name: "AssociatedPassengerBookingDetailsId",
                table: "PassengerBookingDetails",
                newName: "AssociatedPassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerBookingDetails_AssociatedPassengerBookingDetailsId",
                table: "PassengerBookingDetails",
                newName: "IX_PassengerBookingDetails_AssociatedPassengerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassengerId",
                table: "PassengerBookingDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingDetailsId",
                table: "Passengers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_AssociatedPassengerId",
                table: "Passengers",
                column: "AssociatedPassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
