using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_PK_ApisData_to_Guid_step2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_APISData",
                table: "APISData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "APISData");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "APISData",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_APISData",
                table: "APISData",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_APISData",
                table: "APISData");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "APISData",
                newName: "NewId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "APISData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_APISData",
                table: "APISData",
                column: "NewId");
        }
    }
}
