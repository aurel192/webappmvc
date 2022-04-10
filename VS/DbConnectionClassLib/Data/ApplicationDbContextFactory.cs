using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionClassLib.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private static string _connectionString;

        public ApplicationDbContext CreateDbContext()
        {
            return CreateDbContext(null);
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = ConnectionString.Get();
            }

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySql(_connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}
