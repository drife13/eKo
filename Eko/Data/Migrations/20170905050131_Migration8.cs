using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eko.Data.Migrations
{
    public partial class Migration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Models_ModelID",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "ModelID",
                table: "Sales",
                newName: "ModelId");

            migrationBuilder.RenameColumn(
                name: "ItemID",
                table: "Sales",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_ModelID",
                table: "Sales",
                newName: "IX_Sales_ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Models_ModelId",
                table: "Sales",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Models_ModelId",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Sales",
                newName: "ModelID");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Sales",
                newName: "ItemID");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_ModelId",
                table: "Sales",
                newName: "IX_Sales_ModelID");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Sales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Models_ModelID",
                table: "Sales",
                column: "ModelID",
                principalTable: "Models",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
