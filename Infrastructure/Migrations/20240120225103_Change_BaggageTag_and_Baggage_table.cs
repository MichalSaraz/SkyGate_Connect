using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_BaggageTag_and_Baggage_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TagNumbers",
                table: "TagNumbers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TagNumbers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Baggage");

            migrationBuilder.DropColumn(
                name: "IsSpecialBag",
                table: "Baggage");

            migrationBuilder.AlterColumn<string>(
                name: "TagNumber",
                table: "TagNumbers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AirlinePrefix",
                table: "Airlines",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagNumbers",
                table: "TagNumbers",
                column: "TagNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TagNumbers_BaggageId",
                table: "TagNumbers",
                column: "BaggageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TagNumbers",
                table: "TagNumbers");

            migrationBuilder.DropIndex(
                name: "IX_TagNumbers_BaggageId",
                table: "TagNumbers");

            migrationBuilder.AlterColumn<string>(
                name: "TagNumber",
                table: "TagNumbers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TagNumbers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Baggage",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialBag",
                table: "Baggage",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "AirlinePrefix",
                table: "Airlines",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagNumbers",
                table: "TagNumbers",
                column: "BaggageId");
        }
    }
}
