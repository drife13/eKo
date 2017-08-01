using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eko.Data.Migrations
{
    public partial class Migration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_UserID",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchListItems_AspNetUsers_UserID",
                table: "WatchListItems");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "WatchListItems",
                newName: "ApplicationUserID");

            migrationBuilder.RenameColumn(
                name: "BuyerID",
                table: "Orders",
                newName: "ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BuyerID",
                table: "Orders",
                newName: "IX_Orders_ApplicationUserID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "CartItems",
                newName: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_ApplicationUserID",
                table: "CartItems",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserID",
                table: "Orders",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchListItems_AspNetUsers_ApplicationUserID",
                table: "WatchListItems",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_ApplicationUserID",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchListItems_AspNetUsers_ApplicationUserID",
                table: "WatchListItems");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "WatchListItems",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "Orders",
                newName: "BuyerID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ApplicationUserID",
                table: "Orders",
                newName: "IX_Orders_BuyerID");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "CartItems",
                newName: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_UserID",
                table: "CartItems",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerID",
                table: "Orders",
                column: "BuyerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchListItems_AspNetUsers_UserID",
                table: "WatchListItems",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
