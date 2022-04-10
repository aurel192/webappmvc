using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbConnectionClassLib.Migrations
{
    public partial class AlphaVantageTSDA_CRYPTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "av_digital_currency_daily",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    lastupdate = table.Column<DateTime>(nullable: true),
                    market = table.Column<string>(maxLength: 200, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    state = table.Column<int>(nullable: false),
                    symbol = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_av_digital_currency_daily", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "av_time_series_daily_adjusted",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    lastupdate = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    state = table.Column<int>(nullable: false),
                    symbol = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_av_time_series_daily_adjusted", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "av_digital_currency_daily_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    avdcdId = table.Column<int>(nullable: false),
                    closeCNY = table.Column<double>(nullable: false),
                    closeUSD = table.Column<double>(nullable: false),
                    highCNY = table.Column<double>(nullable: false),
                    highUSD = table.Column<double>(nullable: false),
                    lowCNY = table.Column<double>(nullable: false),
                    lowUSD = table.Column<double>(nullable: false),
                    marketcapUSD = table.Column<double>(nullable: false),
                    openCNY = table.Column<double>(nullable: false),
                    openUSD = table.Column<double>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false),
                    volume = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_av_digital_currency_daily_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_av_digital_currency_daily_data_av_digital_currency_daily_avdcdId",
                        column: x => x.avdcdId,
                        principalTable: "av_digital_currency_daily",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "av_time_series_daily_adjusted_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AV_DIGITAL_CURRENCY_DAILYid = table.Column<int>(nullable: true),
                    adjusted_close = table.Column<double>(nullable: false),
                    avtsdaId = table.Column<int>(nullable: false),
                    close = table.Column<double>(nullable: false),
                    dividend_amount = table.Column<double>(nullable: false),
                    high = table.Column<double>(nullable: false),
                    low = table.Column<double>(nullable: false),
                    open = table.Column<double>(nullable: false),
                    split_coefficient = table.Column<double>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false),
                    volume = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_av_time_series_daily_adjusted_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_av_time_series_daily_adjusted_data_av_digital_currency_daily_AV_DIGITAL_CURRENCY_DAILYid",
                        column: x => x.AV_DIGITAL_CURRENCY_DAILYid,
                        principalTable: "av_digital_currency_daily",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_av_time_series_daily_adjusted_data_av_time_series_daily_adjusted_avtsdaId",
                        column: x => x.avtsdaId,
                        principalTable: "av_time_series_daily_adjusted",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_av_digital_currency_daily_data_avdcdId",
                table: "av_digital_currency_daily_data",
                column: "avdcdId");

            migrationBuilder.CreateIndex(
                name: "IX_av_time_series_daily_adjusted_data_AV_DIGITAL_CURRENCY_DAILYid",
                table: "av_time_series_daily_adjusted_data",
                column: "AV_DIGITAL_CURRENCY_DAILYid");

            migrationBuilder.CreateIndex(
                name: "IX_av_time_series_daily_adjusted_data_avtsdaId",
                table: "av_time_series_daily_adjusted_data",
                column: "avtsdaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "av_digital_currency_daily_data");

            migrationBuilder.DropTable(
                name: "av_time_series_daily_adjusted_data");

            migrationBuilder.DropTable(
                name: "av_digital_currency_daily");

            migrationBuilder.DropTable(
                name: "av_time_series_daily_adjusted");
        }
    }
}
