using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Migrations
{
    public partial class PortfolioAllampapir : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "portfolio_allampapir",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    lastupdate = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    state = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_allampapir", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_allampapir_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllampapirId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_allampapir_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_portfolio_allampapir_data_portfolio_allampapir_AllampapirId",
                        column: x => x.AllampapirId,
                        principalTable: "portfolio_allampapir",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_allampapir_data_AllampapirId",
                table: "portfolio_allampapir_data",
                column: "AllampapirId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "portfolio_allampapir_data");

            migrationBuilder.DropTable(
                name: "portfolio_allampapir");
        }
    }
}
