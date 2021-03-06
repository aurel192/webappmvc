// <auto-generated />
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
    [Migration("20180325091809_PortfolioAllampapir")]
    partial class PortfolioAllampapir
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
