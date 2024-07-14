using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_ActionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionHistory<object>",
                table: "ActionHistory<object>");

            migrationBuilder.RenameTable(
                name: "ActionHistory<object>",
                newName: "ActionHistory");

            migrationBuilder.RenameColumn(
                name: "SerializedDetails",
                table: "ActionHistory",
                newName: "SerializedOldValue");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "ActionHistory",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerOrItemId",
                table: "ActionHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SerializedNewValue",
                table: "ActionHistory",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionHistory",
                table: "ActionHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ActionHistory_PassengerOrItemId",
                table: "ActionHistory",
                column: "PassengerOrItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionHistory_Passengers_PassengerOrItemId",
                table: "ActionHistory",
                column: "PassengerOrItemId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionHistory_Passengers_PassengerOrItemId",
                table: "ActionHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionHistory",
                table: "ActionHistory");

            migrationBuilder.DropIndex(
                name: "IX_ActionHistory_PassengerOrItemId",
                table: "ActionHistory");

            migrationBuilder.DropColumn(
                name: "PassengerOrItemId",
                table: "ActionHistory");

            migrationBuilder.DropColumn(
                name: "SerializedNewValue",
                table: "ActionHistory");

            migrationBuilder.RenameTable(
                name: "ActionHistory",
                newName: "ActionHistory<object>");

            migrationBuilder.RenameColumn(
                name: "SerializedOldValue",
                table: "ActionHistory<object>",
                newName: "SerializedDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "ActionHistory<object>",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionHistory<object>",
                table: "ActionHistory<object>",
                column: "Id");
        }
    }
}
