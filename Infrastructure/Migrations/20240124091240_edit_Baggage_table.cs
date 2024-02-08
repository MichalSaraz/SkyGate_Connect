using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_Baggage_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagNumbers");

            migrationBuilder.AddColumn<string>(
                name: "DestinationId",
                table: "Baggage",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagNumber",
                table: "Baggage",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baggage_DestinationId",
                table: "Baggage",
                column: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baggage_Destinations_DestinationId",
                table: "Baggage",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "IATAAirportCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baggage_Destinations_DestinationId",
                table: "Baggage");

            migrationBuilder.DropIndex(
                name: "IX_Baggage_DestinationId",
                table: "Baggage");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "Baggage");

            migrationBuilder.DropColumn(
                name: "TagNumber",
                table: "Baggage");

            migrationBuilder.CreateTable(
                name: "TagNumbers",
                columns: table => new
                {
                    TagNumber = table.Column<string>(type: "text", nullable: false),
                    AirlineId = table.Column<string>(type: "text", nullable: true),
                    BaggageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagNumbers", x => x.TagNumber);
                    table.ForeignKey(
                        name: "FK_TagNumbers_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "CarrierCode");
                    table.ForeignKey(
                        name: "FK_TagNumbers_Baggage_BaggageId",
                        column: x => x.BaggageId,
                        principalTable: "Baggage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagNumbers_AirlineId",
                table: "TagNumbers",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_TagNumbers_BaggageId",
                table: "TagNumbers",
                column: "BaggageId",
                unique: true);
        }
    }
}
