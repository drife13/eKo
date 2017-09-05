using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Eko.Data.Migrations
{
    public partial class Migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ProductHistories_ProductHistoryID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ProductHistories");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductHistoryID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductHistoryID",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    ModelID = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sales_Models_ModelID",
                        column: x => x.ModelID,
                        principalTable: "Models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ModelID",
                table: "Sales",
                column: "ModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "ProductHistoryID",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductHistories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    ModelID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductHistories_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductHistories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductHistories_Models_ModelID",
                        column: x => x.ModelID,
                        principalTable: "Models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductHistoryID",
                table: "Orders",
                column: "ProductHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductHistories_BrandID",
                table: "ProductHistories",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductHistories_CategoryID",
                table: "ProductHistories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductHistories_ModelID",
                table: "ProductHistories",
                column: "ModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ProductHistories_ProductHistoryID",
                table: "Orders",
                column: "ProductHistoryID",
                principalTable: "ProductHistories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
