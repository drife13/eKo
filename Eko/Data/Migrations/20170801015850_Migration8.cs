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
                name: "FK_Items_Orders_OrderId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Items",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_Items_OrderId",
                table: "Items",
                newName: "IX_Items_OrderID");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Items",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_OrderID",
                table: "Items",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_OrderID",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "Items",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_OrderID",
                table: "Items",
                newName: "IX_Items_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_OrderId",
                table: "Items",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
