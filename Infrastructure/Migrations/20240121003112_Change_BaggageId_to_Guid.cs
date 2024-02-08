using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_BaggageId_to_Guid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Baggage_BaggageId",
                table: "FlightBaggage");

            // Drop primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Baggage",
                table: "Baggage");

            // Drop the existing Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Baggage");

            // Add the new Id column as Guid
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Baggage",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            // Set the new Id column as primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Baggage",
                table: "Baggage",
                column: "Id");

            // Change the type of BaggageId column in FlightBaggage table
            migrationBuilder.AlterColumn<Guid>(
                name: "BaggageId",
                table: "FlightBaggage",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            // Add foreign key constraint back
            migrationBuilder.AddForeignKey(
                name: "FK_FlightBaggage_Baggage_BaggageId",
                table: "FlightBaggage",
                column: "BaggageId",
                principalTable: "Baggage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBaggage_Baggage_BaggageId",
                table: "FlightBaggage");

            // Drop primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Baggage",
                table: "Baggage");

            // Drop the existing Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Baggage");

            // Add the new Id column as integer
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Baggage",
                type: "integer",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            // Set the new Id column as primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Baggage",
                table: "Baggage",
                column: "Id");

            // Change the type of BaggageId column in FlightBaggage table
            migrationBuilder.AlterColumn<int>(
                name: "BaggageId",
                table: "FlightBaggage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            // Add foreign key constraint back
            migrationBuilder.AddForeignKey(
                name: "FK_FlightBaggage_Baggage_BaggageId",
                table: "FlightBaggage",
                column: "BaggageId",
                principalTable: "Baggage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
