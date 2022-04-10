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

namespace WebAppMVC.ApiControllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/RemoveFavorite")]
    public class RemoveFavoriteController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public RemoveFavoriteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<RemoveFavoriteController> logger)
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<List<Favorite>> Post(Favorite f)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var removeFav = db.Favorites.Where(x => x.UserId == user.Id && x.Type == f.Type).FirstOrDefault();
                if (removeFav != null)
                {
                    db.Favorites.Remove(removeFav);
                    db.SaveChanges();
                }
                var favorites = db.Favorites.Where(fav => fav.UserId == user.Id).ToList();
                return favorites;
            }
            catch (Exception ex)
            {
                ApplicationUser user = null;
                try { user = await _userManager.GetUserAsync(HttpContext.User); }
                catch (Exception) { }
                Helper.LogException(ex, Request, user);
                throw ex;
            }
        }
    }
}