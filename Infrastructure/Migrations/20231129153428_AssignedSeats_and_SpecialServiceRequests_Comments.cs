using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignedSeats_and_SpecialServiceRequests_Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassengerComment");

            migrationBuilder.DropTable(
                name: "PassengerFlightDetails");

            migrationBuilder.DropTable(
                name: "Seat");           

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);           

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PassengerId",
                table: "Comments",
                column: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Passengers_PassengerId",
                table: "Comments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);                   
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Passengers_PassengerId",
                table: "Comments");           

            migrationBuilder.DropIndex(
                name: "IX_Comments_PassengerId",
                table: "Comments");            

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Comments");

           

            migrationBuilder.CreateTable(
                name: "PassengerComment",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerComment", x => new { x.PassengerId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_PassengerComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerComment_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerFlightDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    SeatId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerFlightDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassengerFlightDetails_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerFlightDetails_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                 name: "Seat",
                 columns: table => new
                 {
                     Id = table.Column<int>(type: "integer", nullable: false)
                         .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                     PassengerId = table.Column<int>(type: "integer", nullable: false),
                     FlightId = table.Column<int>(type: "integer", nullable: false),
                     SeatNumber = table.Column<string>(type: "text", nullable: true),
                     SeatType = table.Column<string>(type: "text", nullable: false),
                     FlightClass = table.Column<string>(type: "text", nullable: false),
                     SeatStatus = table.Column<string>(type: "text", nullable: false)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_Seat", x => x.Id);
                     table.ForeignKey(
                         name: "FK_Seat_Passengers_PassengerId",
                         column: x => x.PassengerId,
                         principalTable: "Passengers",
                         principalColumn: "Id",
                         onDelete: ReferentialAction.Cascade);
                     table.ForeignKey(
                         name: "FK_Seat_Flight_FlightId",
                         column: x => x.FlightId,
                         principalTable: "Flights",
                         principalColumn: "Id",
                         onDelete: ReferentialAction.Cascade);
                 });
        }
    }
}
