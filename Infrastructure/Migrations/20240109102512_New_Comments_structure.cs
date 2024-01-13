using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class New_Comments_structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedSeats",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "Flights",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "SpecialServiceRequests",
                table: "Passengers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsMarkedAsRead",
                table: "Comments",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "PredefinedCommentId",
                table: "Comments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PredefinedComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedComments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PredefinedCommentId",
                table: "Comments",
                column: "PredefinedCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_PredefinedComments_PredefinedCommentId",
                table: "Comments",
                column: "PredefinedCommentId",
                principalTable: "PredefinedComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_PredefinedComments_PredefinedCommentId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "PredefinedComments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PredefinedCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PredefinedCommentId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "AssignedSeats",
                table: "Passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flights",
                table: "Passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialServiceRequests",
                table: "Passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMarkedAsRead",
                table: "Comments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
