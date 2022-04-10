namespace DbConnectionClassLib.Data
{
    public class DatabaseInstance
    {
        private static ApplicationDbContext dbinstance;

        public static ApplicationDbContext Db
        {
            get
            {
                if (dbinstance == null)
                {
                    dbinstance = new ApplicationDbContextFactory().CreateDbContext();
                }
                return dbinstance;
            }
        }
        
        public static void DbRefresh()
        {
            dbinstance = new ApplicationDbContextFactory().CreateDbContext();
        }
    }
}
