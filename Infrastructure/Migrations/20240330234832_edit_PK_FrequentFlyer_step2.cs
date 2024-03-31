using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_PK_FrequentFlyer_step2 : Migration
    {
        /// <inheritdoc />
        /// 
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FrequentFlyerCards");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "FrequentFlyerCards",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "PassengerBookingDetails",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "PassengerBookingDetails");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FrequentFlyerCards",
                newName: "NewId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FrequentFlyerCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FrequentFlyerCards",
                table: "FrequentFlyerCards",
                column: "NewId");
        }        
    }
}
