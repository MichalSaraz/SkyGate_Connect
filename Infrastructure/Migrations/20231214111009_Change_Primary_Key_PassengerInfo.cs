using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_Primary_Key_PassengerInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APISData_Passengers_PassengerId",
                table: "APISData");

            migrationBuilder.DropForeignKey(
                name: "FK_Baggage_Passengers_PassengerId",
                table: "Baggage");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Passengers_PassengerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
                table: "FrequentFlyers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_PassengerInfos_Id",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passengers",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassengerInfos",
                table: "PassengerInfos");

            migrationBuilder.DropIndex(
                name: "IX_APISData_PassengerId",
                table: "APISData");

            migrationBuilder.DropIndex(
                name: "IX_Baggage_PassengerId",
                table: "Baggage");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PassengerId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_FrequentFlyers_PassengerInfoId",
                table: "FrequentFlyers");

            migrationBuilder.RenameColumn(
                name: "Id", 
                table: "PassengerInfos",
                newName: "OldId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PassengerInfos",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "APISData",
                newName: "OldPassengerId");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "Baggage",
                newName: "OldPassengerId");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "Comments",
                newName: "OldPassengerId");

            migrationBuilder.RenameColumn(
                name: "PassengerInfoId",
                table: "FrequentFlyers",
                newName: "OldPassengerInfoId");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "PassengerFlight",
                newName: "OldPassengerId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Passengers",
                newName: "OldId");

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "APISData",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "Baggage",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
               name: "PassengerInfoId",
               table: "FrequentFlyers",
               nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "PassengerFlight",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Passengers",
                nullable: true);

            migrationBuilder.Sql(
                @"UPDATE ""PassengerInfos"" SET ""Id"" = uuid_generate_v4()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passengers",
                table: "Passengers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassengerInfos",
                table: "PassengerInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_APISData_Passengers_PassengerId",
                table: "APISData",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baggage_Passengers_PassengerId",
                table: "Baggage",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Passengers_PassengerId",
                table: "Comments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
                table: "FrequentFlyers",
                column: "PassengerInfoId",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerFlight_Passengers_PassengerId",
                table: "PassengerFlight",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_PassengerInfos_Id",
                table: "Passengers",
                column: "Id",
                principalTable: "PassengerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_APISData_PassengerId",
                table: "APISData",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Baggage_PassengerId",
                table: "Baggage",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PassengerId",
                table: "Comments",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequentFlyers_PassengerInfoId",
                table: "FrequentFlyers",
                column: "PassengerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerFlight_PassengerId",
                table: "PassengerFlight",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_Id",
                table: "Passengers",
                column: "Id");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "PassengerInfos");           

            migrationBuilder.DropColumn(
                name: "OldPassengerId",
                table: "PassengerFlight");            

            migrationBuilder.DropColumn(
                 name: "OldPassengerInfoId",
                 table: "FrequentFlyers");           

            migrationBuilder.DropColumn(
                 name: "OldPassengerId",
                 table: "Comments");            

            migrationBuilder.DropColumn(
                 name: "OldPassengerId",
                 table: "Baggage");            

            migrationBuilder.DropColumn(
                 name: "OldPassengerId",
                 table: "APISData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_APISData_Passengers_PassengerId",
            //    table: "APISData");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Baggage_Passengers_PassengerId",
            //    table: "Baggage");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Comments_Passengers_PassengerId",
            //    table: "Comments");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
            //    table: "FrequentFlyers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_PassengerFlight_Passengers_PassengerId",
            //    table: "PassengerFlight");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Passengers_PassengerInfos_NewId",
            //    table: "Passengers");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Passengers",
            //    table: "Passengers");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_PassengerInfos",
            //    table: "PassengerInfos");

            //migrationBuilder.DropColumn(
            //    name: "NewId",
            //    table: "Passengers");

            //migrationBuilder.DropColumn(
            //    name: "NewId",
            //    table: "PassengerInfos");

            //migrationBuilder.AddColumn<int>(
            //    name: "Id",
            //    table: "Passengers",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "PassengerInfos",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "integer")
            //    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            //migrationBuilder.DropColumn(
            //    name: "PassengerId",
            //    table: "PassengerFlight");

            //migrationBuilder.AddColumn<int>(
            //    name: "PassengerId",
            //    table: "PassengerFlight",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.DropColumn(
            //    name: "PassengerInfoId",
            //    table: "FrequentFlyers");

            //migrationBuilder.AddColumn<int>(
            //    name: "PassengerInfoId",
            //    table: "FrequentFlyers",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.DropColumn(
            //    name: "PassengerId",
            //    table: "Comments");

            //migrationBuilder.AddColumn<int>(
            //    name: "PassengerId",
            //    table: "Comments",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.DropColumn(
            //    name: "PassengerId",
            //    table: "Baggage");

            //migrationBuilder.AddColumn<int>(
            //    name: "PassengerId",
            //    table: "Baggage",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.DropColumn(
            //    name: "PassengerId",
            //    table: "APISData");

            //migrationBuilder.AddColumn<int>(
            //    name: "PassengerId",
            //    table: "APISData",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Passengers",
            //    table: "Passengers",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_PassengerInfos",
            //    table: "PassengerInfos",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_APISData_Passengers_PassengerId",
            //    table: "APISData",
            //    column: "PassengerId",
            //    principalTable: "Passengers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Baggage_Passengers_PassengerId",
            //    table: "Baggage",
            //    column: "PassengerId",
            //    principalTable: "Passengers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comments_Passengers_PassengerId",
            //    table: "Comments",
            //    column: "PassengerId",
            //    principalTable: "Passengers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FrequentFlyers_PassengerInfos_PassengerInfoId",
            //    table: "FrequentFlyers",
            //    column: "PassengerInfoId",
            //    principalTable: "PassengerInfos",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PassengerFlight_Passengers_PassengerId",
            //    table: "PassengerFlight",
            //    column: "PassengerId",
            //    principalTable: "Passengers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Passengers_PassengerInfos_Id",
            //    table: "Passengers",
            //    column: "Id",
            //    principalTable: "PassengerInfos",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
   