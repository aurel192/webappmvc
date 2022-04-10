using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Migrations
{
    public partial class av_symbols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "av_symbols",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    currency = table.Column<string>(maxLength: 50, nullable: true),
                    marketClose = table.Column<string>(maxLength: 10, nullable: true),
                    marketOpen = table.Column<string>(maxLength: 10, nullable: true),
                    name = table.Column<string>(maxLength: 100, nullable: true),
                    region = table.Column<string>(maxLength: 100, nullable: true),
                    symbol = table.Column<string>(maxLength: 20, nullable: true),
                    timezone = table.Column<string>(maxLength: 50, nullable: true),
                    type = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_av_symbols", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "av_symbols");
        }
    }
}
