using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eko.Data.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Items",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.CreateTable(
                name: "WatchListItems",
                columns: table => new
                {
                    ApplicationUserID = table.Column<string>(nullable: false),
                    ItemID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListItems", x => new { x.ApplicationUserID, x.ItemID });
                    table.ForeignKey(
                        name: "FK_WatchListItems_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchListItems_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchListItems_ItemID",
                table: "WatchListItems",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchListItems");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Items",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
