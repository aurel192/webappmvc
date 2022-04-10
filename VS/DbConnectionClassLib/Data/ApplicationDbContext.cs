using DbConnectionClassLib.Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
/*
PM:
Add-Migration -p DbConnectionClassLib Init
Add-Migration -p DbConnectionClassLib InstrTypesAndPortfolioTables
Add-Migration -p DbConnectionClassLib PortfolioTablesLastUpdate
Add-Migration -p DbConnectionClassLib PortfolioAllampapir
Add-Migration -p DbConnectionClassLib AlphaVantageTSDA_CRYPTO
Add-Migration -p DbConnectionClassLib AlphaVantageFix
Add-Migration -p DbConnectionClassLib Favorites
Add-Migration -p DbConnectionClassLib FavoritesFixed
Add-Migration -p DbConnectionClassLib av_symbols
Add-Migration -p DbConnectionClassLib db_settings_and_db_log
Add-Migration -p DbConnectionClassLib av_time_series_intraday_data
Add-Migration -p DbConnectionClassLib av_overview
Update-Database
*/
namespace DbConnectionClassLib.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<InstrumentType> InstrumentTypes { get; set; }
        public DbSet<portfolio_hunstock> portfolio_hunstock { get; set; }
        public DbSet<portfolio_hunstock_data> portfolio_hunstock_data { get; set; }
        public DbSet<portfolio_hunfund> portfolio_hunfund { get; set; }
        public DbSet<portfolio_hunfund_data> portfolio_hunfund_data { get; set; }
        public DbSet<portfolio_allampapir> portfolio_allampapir { get; set; }
        public DbSet<portfolio_allampapir_data> portfolio_allampapir_data { get; set; }
        public DbSet<AV_TIME_SERIES_DAILY_ADJUSTED> av_time_series_daily_adjusted { get; set; }
        public DbSet<AV_TIME_SERIES_DAILY_ADJUSTED_DATA> av_time_series_daily_adjusted_data { get; set; }
        public DbSet<AV_TIME_SERIES_INTRADAY_DATA> av_time_series_intraday_data { get; set; }
        public DbSet<AV_DIGITAL_CURRENCY_DAILY> av_digital_currency_daily { get; set; }
        public DbSet<AV_DIGITAL_CURRENCY_DAILY_DATA> av_digital_currency_daily_data { get; set; }
        public DbSet<AV_OVERVIEW> av_overview { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AV_SYMBOL> av_symbols { get; set; }
        public DbSet<DBSettings> db_settings { get; set; }
        public DbSet<DBLog> db_log { get; set; }
    }
}
