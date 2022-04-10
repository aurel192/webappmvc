using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Migrations
{
    public partial class PortfolioTablesLastUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastupdate",
                table: "portfolio_hunstock",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastupdate",
                table: "portfolio_hunfund",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastupdate",
                table: "portfolio_hunstock");

            migrationBuilder.DropColumn(
                name: "lastupdate",
                table: "portfolio_hunfund");
        }
    }
}
