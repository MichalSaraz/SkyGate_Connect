using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Type_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Passengers_PassengerId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "Comments",
                newName: "PassengerOrItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PassengerId",
                table: "Comments",
                newName: "IX_Comments_PassengerOrItemId");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ActionHistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Passengers_PassengerOrItemId",
                table: "Comments",
                column: "PassengerOrItemId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Passengers_PassengerOrItemId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ActionHistory");

            migrationBuilder.RenameColumn(
                name: "PassengerOrItemId",
                table: "Comments",
                newName: "PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PassengerOrItemId",
                table: "Comments",
                newName: "IX_Comments_PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Passengers_PassengerId",
                table: "Comments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
