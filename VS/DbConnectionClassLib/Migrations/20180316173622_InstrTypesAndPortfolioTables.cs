using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Migrations
{
    public partial class InstrTypesAndPortfolioTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstrumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_hunfund",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    state = table.Column<int>(nullable: false),
                    ticker = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_hunfund", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_hunstock",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: true),
                    state = table.Column<int>(nullable: false),
                    ticker = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_hunstock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_hunfund_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Capitalization = table.Column<double>(nullable: false),
                    Cashflow = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    YearlyYield = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_hunfund_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_portfolio_hunfund_data_portfolio_hunfund_FundId",
                        column: x => x.FundId,
                        principalTable: "portfolio_hunfund",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_hunstock_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Avg = table.Column<double>(nullable: false),
                    Close = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    High = table.Column<double>(nullable: false),
                    Low = table.Column<double>(nullable: false),
                    Open = table.Column<double>(nullable: false),
                    StockId = table.Column<int>(nullable: false),
                    Volume = table.Column<double>(nullable: false),
                    VolumeCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_hunstock_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_portfolio_hunstock_data_portfolio_hunstock_StockId",
                        column: x => x.StockId,
                        principalTable: "portfolio_hunstock",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_hunfund_data_FundId",
                table: "portfolio_hunfund_data",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_hunstock_data_StockId",
                table: "portfolio_hunstock_data",
                column: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstrumentTypes");

            migrationBuilder.DropTable(
                name: "portfolio_hunfund_data");

            migrationBuilder.DropTable(
                name: "portfolio_hunstock_data");

            migrationBuilder.DropTable(
                name: "portfolio_hunfund");

            migrationBuilder.DropTable(
                name: "portfolio_hunstock");
        }
    }
}
