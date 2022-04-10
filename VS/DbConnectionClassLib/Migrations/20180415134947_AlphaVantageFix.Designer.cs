﻿// <auto-generated />
using DbConnectionClassLib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DbConnectionClassLib.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180415134947_AlphaVantageFix")]
    partial class AlphaVantageFix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("DbConnectionClassLib.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.AV_DIGITAL_CURRENCY_DAILY", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("lastupdate");

                    b.Property<string>("market")
                        .HasMaxLength(200);

                    b.Property<string>("name")
                        .HasMaxLength(200);

                    b.Property<int>("state");

                    b.Property<string>("symbol")
                        .HasMaxLength(200);

                    b.HasKey("id");

                    b.ToTable("av_digital_currency_daily");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.AV_DIGITAL_CURRENCY_DAILY_DATA", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("avdcdId");

                    b.Property<double>("closeCNY");

                    b.Property<double>("closeUSD");

                    b.Property<double>("highCNY");

                    b.Property<double>("highUSD");

                    b.Property<double>("lowCNY");

                    b.Property<double>("lowUSD");

                    b.Property<double>("marketcapUSD");

                    b.Property<double>("openCNY");

                    b.Property<double>("openUSD");

                    b.Property<DateTime>("timestamp");

                    b.Property<double>("volume");

                    b.HasKey("id");

                    b.HasIndex("avdcdId");

                    b.ToTable("av_digital_currency_daily_data");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.AV_TIME_SERIES_DAILY_ADJUSTED", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("lastupdate");

                    b.Property<string>("name")
                        .HasMaxLength(200);

                    b.Property<int>("state");

                    b.Property<string>("symbol")
                        .HasMaxLength(200);

                    b.HasKey("id");

                    b.ToTable("av_time_series_daily_adjusted");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.AV_TIME_SERIES_DAILY_ADJUSTED_DATA", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AV_DIGITAL_CURRENCY_DAILYid");

                    b.Property<double>("adjusted_close");

                    b.Property<int>("avtsdaId");

                    b.Property<double>("close");

                    b.Property<double>("dividend_amount");

                    b.Property<double>("high");

                    b.Property<double>("low");

                    b.Property<double>("open");

                    b.Property<double>("split_coefficient");

                    b.Property<DateTime>("timestamp");

                    b.Property<double>("volume");

                    b.HasKey("id");

                    b.HasIndex("AV_DIGITAL_CURRENCY_DAILYid");

                    b.HasIndex("avtsdaId");

                    b.ToTable("av_time_series_daily_adjusted_data");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.InstrumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("InstrumentTypes");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_allampapir", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("code");

                    b.Property<DateTime?>("lastupdate");

                    b.Property<string>("name")
                        .HasMaxLength(200);

                    b.Property<int>("state");

                    b.HasKey("id");

                    b.ToTable("portfolio_allampapir");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_allampapir_data", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AllampapirId");

                    b.Property<DateTime>("Date");

                    b.Property<double>("Price");

                    b.HasKey("id");

                    b.HasIndex("AllampapirId");

                    b.ToTable("portfolio_allampapir_data");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_hunfund", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("lastupdate");

                    b.Property<string>("name")
                        .HasMaxLength(200);

                    b.Property<int>("state");

                    b.Property<int>("ticker");

                    b.HasKey("id");

                    b.ToTable("portfolio_hunfund");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_hunfund_data", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Capitalization");

                    b.Property<double>("Cashflow");

                    b.Property<DateTime>("Date");

                    b.Property<int>("FundId");

                    b.Property<double>("Price");

                    b.Property<double>("YearlyYield");

                    b.HasKey("id");

                    b.HasIndex("FundId");

                    b.ToTable("portfolio_hunfund_data");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_hunstock", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("lastupdate");

                    b.Property<string>("name")
                        .HasMaxLength(50);

                    b.Property<int>("state");

                    b.Property<int>("ticker");

                    b.HasKey("id");

                    b.ToTable("portfolio_hunstock");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_hunstock_data", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Avg");

                    b.Property<double>("Close");

                    b.Property<DateTime>("Date");

                    b.Property<double>("High");

                    b.Property<double>("Low");

                    b.Property<double>("Open");

                    b.Property<int>("StockId");

                    b.Property<double>("Volume");

                    b.Property<int>("VolumeCount");

                    b.HasKey("id");

                    b.HasIndex("StockId");

                    b.ToTable("portfolio_hunstock_data");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.AV_DIGITAL_CURRENCY_DAILY_DATA", b =>
                {
                    b.HasOne("DbConnectionClassLib.Tables.AV_DIGITAL_CURRENCY_DAILY", "AV_DigitalCurrencyDaily")
                        .WithMany()
                        .HasForeignKey("avdcdId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.AV_TIME_SERIES_DAILY_ADJUSTED_DATA", b =>
                {
                    b.HasOne("DbConnectionClassLib.Tables.AV_DIGITAL_CURRENCY_DAILY")
                        .WithMany("Data")
                        .HasForeignKey("AV_DIGITAL_CURRENCY_DAILYid");

                    b.HasOne("DbConnectionClassLib.Tables.AV_TIME_SERIES_DAILY_ADJUSTED", "AV_TimeSeriesDailyAdjusted")
                        .WithMany("Data")
                        .HasForeignKey("avtsdaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_allampapir_data", b =>
                {
                    b.HasOne("DbConnectionClassLib.Tables.portfolio_allampapir", "Portfolio_allampapir")
                        .WithMany("Data")
                        .HasForeignKey("AllampapirId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_hunfund_data", b =>
                {
                    b.HasOne("DbConnectionClassLib.Tables.portfolio_hunfund", "Portfolio_hunfund")
                        .WithMany("Data")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DbConnectionClassLib.Tables.portfolio_hunstock_data", b =>
                {
                    b.HasOne("DbConnectionClassLib.Tables.portfolio_hunstock", "Portfolio_hunstock")
                        .WithMany("Data")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DbConnectionClassLib.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DbConnectionClassLib.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DbConnectionClassLib.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DbConnectionClassLib.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
