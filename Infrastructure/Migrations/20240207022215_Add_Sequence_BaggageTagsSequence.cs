using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sequence_BaggageTagsSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "BaggageTagsSequence",
                maxValue: 999999L);

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Baggage",
                type: "integer",
                nullable: true,
                defaultValueSql: "nextval('\"BaggageTagsSequence\"')",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "BaggageTagsSequence");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Baggage",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValueSql: "nextval('\"BaggageTagsSequence\"')");
        }
    }
}
