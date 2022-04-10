using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionClassLib.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMVC.Services;
using HelperClassLib;
using Microsoft.AspNetCore.Authorization;
using static HelperClassLib.Helpers.HelperClass;

namespace WebAppMVC.Controllers
{
    [Authorize]
    public class CompareController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public CompareController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<CompareController> logger)
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult Index()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return View(Helper.GetInstrumentListViewListItems(user));
        }

    }
}