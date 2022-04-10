using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionClassLib.Data;
using DbConnectionClassLib.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMVC.Services;
using static HelperClassLib.Helpers.HelperClass;

// http://ftm.collectioninventory.com/Account/Login
// http://ftm.collectioninventory.com/api/GetDbLog

namespace WebAppMVC.ApiControllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/GetDbLog")]
    public class GetDbLog : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public GetDbLog(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<GetDbLog> logger)
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _logger = logger;
        }

        public ContentResult Get()
        {
            try
            {
                string strResult = "";
                string newLine = "\n";
                List<string> result = new List<string>();
                if (this.User.IsInRole("ADMIN"))
                {
                    //DbConnectionClassLib.Data.SqlCommands.Get_DbLog(); // TODO Audit
                    DateTime from = DateTime.Today.AddDays(-7);
                    List<string> res = db.db_log
                                      .Where(l => l.TimeStamp > from)
                                      .OrderByDescending(l => l.TimeStamp).Select(l => l.TimeStamp.ToString() + newLine + l.Key + newLine + l.Value + newLine + newLine)
                                      .ToList();
                    result.AddRange(res);
                }
                else
                {
                    result.Add("DENIED");
                }
                foreach (string row in result)
                {
                    strResult += row.Replace("&apikey=7JXQ","") + newLine + newLine + newLine;
                }
                strResult += "";
                return Content(strResult);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex, Request);
                throw ex;
            }
        }

    }
}