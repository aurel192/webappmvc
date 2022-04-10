using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionClassLib.Data;
using DbConnectionClassLib.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMVC.Services;

namespace WebAppMVC.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Initialize")]
    public class InitializeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public InitializeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<InitializeController> logger)
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                if (db.InstrumentTypes.Any() == false)
                {
                    db.InstrumentTypes.Add(new InstrumentType { Number = 1, Name = "Otp Funds" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 2, Name = "Equities" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 3, Name = "Indexes" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 4, Name = "Forex" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 5, Name = "Crypto" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 6, Name = "Hungarian Equities" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 7, Name = "Hungarian Mutual Funds" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 8, Name = "Hungarian Equities (BÉT)" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 9, Name = "Commodities" });
                    db.InstrumentTypes.Add(new InstrumentType { Number = 10, Name = "Hungarian MAX Indexes" });
                    db.SaveChanges();
                }

                if(db.portfolio_allampapir.Any() == false)
                {
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "MAX", code = "max" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "RMAX", code = "rmax" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "MAXC", code = "maxc" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "ZMAX", code = "zmax" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "BMX3Y", code = "bmx3y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "BMX5Y", code = "bmx5y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "BMX10Y", code = "bmx10y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "BMX15Y", code = "bmx15y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "3 Hónap", code = "refhozam3m" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "6 Hónap", code = "refhozam6m" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "1 Év", code = "refhozam1y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "3 Év", code = "refhozam3y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "5 Év", code = "refhozam5y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "10 Év", code = "refhozam10y" });
                    db.portfolio_allampapir.Add(new portfolio_allampapir { name = "15 Év", code = "refhozam15y" });
                    db.SaveChanges();
                }

                bool AdminRole = await _roleManager.RoleExistsAsync("ADMIN");
                if (!AdminRole)
                {
                    // first we create Admin rool    
                    var role = new IdentityRole();
                    role.Name = "ADMIN";
                    await _roleManager.CreateAsync(role);

                    //Here we create a Admin super user who will maintain the website
                    var user = new ApplicationUser()
                    {
                        UserName = "admin@ftm.com",
                        Email = "admin@ftm.com"
                    };

                    IdentityResult chkUser = await _userManager.CreateAsync(user, "Qwe123!!!");

                    //Add default User to Role Admin    
                    if (chkUser.Succeeded)
                    {
                        var result1 = await _userManager.AddToRoleAsync(user, "ADMIN");
                    }
                }

                // creating Creating Manager role     
                var ManagerRole = await _roleManager.RoleExistsAsync("Manager");
                if (!ManagerRole)
                {
                    var role = new IdentityRole();
                    role.Name = "Manager";
                    await _roleManager.CreateAsync(role);
                }

                // creating Creating Employee role     
                var EmpRole = await _roleManager.RoleExistsAsync("Employee");
                if (!EmpRole)
                {
                    var role = new IdentityRole();
                    role.Name = "Employee";
                    await _roleManager.CreateAsync(role);
                }
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}