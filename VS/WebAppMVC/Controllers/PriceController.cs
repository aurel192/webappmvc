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
    public class PriceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public PriceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<PriceController> logger)
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

        public ActionResult Search(string arg, string interval, string strFrom = "", string strTo = "")
        {
            // http://ftm.collectioninventory.com/Price/Search/?arg=KO&strFrom=2021-01-01&strTo=2022-01-01
            // http://ftm.collectioninventory.com/Price/Search/?arg=KO&interval=15min
            // http://localhost:50569/Price/Search/?arg=HUN_OTP&strFrom=2021-01-01&strTo=2022-01-01
            // http://localhost:50569/Price/Search/?arg=HUN_OTP&strFrom=2022-01-27&strTo=2022-01-27
            ViewBag.arg = arg; // KO, PEP, MCD, MMM
            ViewBag.interval = interval; // 1min,5min,15min,30min,60min,
            ViewBag.dtFrom = strFrom.ToDate(new DateTime(1900, 1, 1)).ToStr().Replace("-",".");
            ViewBag.dtTo = strTo.ToDate(new DateTime(2099,1,1)).ToStr().Replace("-", ".");
            return View();
        }

    }
}