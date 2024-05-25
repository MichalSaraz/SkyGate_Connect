using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Columns_in_SpecialServiceRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_Passengers_PassengerOrItemId",
                table: "SpecialServiceRequests");

            migrationBuilder.RenameColumn(
                name: "PassengerOrItemId",
                table: "SpecialServiceRequests",
                newName: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequests_Passengers_PassengerId",
                table: "SpecialServiceRequests",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialServiceRequests_Passengers_PassengerId",
                table: "SpecialServiceRequests");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "SpecialServiceRequests",
                newName: "PassengerOrItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialServiceRequests_Passengers_PassengerOrItemId",
                table: "SpecialServiceRequests",
                column: "PassengerOrItemId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
