using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbConnectionClassLib.Data
{
    public class DbInitializer : IDbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                //_context.Database.EnsureCreated();
                var migrations = _context.Database.GetPendingMigrations().Count();
                if (migrations > 0)
                    _context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=== EXC MSG:" + ex.Message);
                Console.WriteLine("=== EXC STTR:" + ex.StackTrace);
                Console.WriteLine("=== EXC SRC:" + ex.Source);
                Console.WriteLine("=== EXC:" + ex.ToString());
                throw ex;
            }
        }

        public void InitializeUsersAndRoles()
        {
            throw new NotImplementedException();
        }
    }
}
