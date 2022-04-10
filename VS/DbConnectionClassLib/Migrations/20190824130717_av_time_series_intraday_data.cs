using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Migrations
{
    public partial class av_time_series_intraday_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "av_time_series_intraday_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    avtsdaId = table.Column<int>(nullable: false),
                    close = table.Column<double>(nullable: false),
                    high = table.Column<double>(nullable: false),
                    low = table.Column<double>(nullable: false),
                    open = table.Column<double>(nullable: false),
                    resoultion = table.Column<string>(maxLength: 10, nullable: true),
                    timestamp = table.Column<DateTime>(nullable: false),
                    volume = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_av_time_series_intraday_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_av_time_series_intraday_data_av_time_series_daily_adjusted_avtsdaId",
                        column: x => x.avtsdaId,
                        principalTable: "av_time_series_daily_adjusted",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_av_time_series_intraday_data_avtsdaId",
                table: "av_time_series_intraday_data",
                column: "avtsdaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "av_time_series_intraday_data");
        }
    }
}
