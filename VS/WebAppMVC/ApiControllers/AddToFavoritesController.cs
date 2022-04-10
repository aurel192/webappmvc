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
    [Route("api/AddToFavorites")]
    public class AddToFavoritesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;

        public AddToFavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<AddToFavoritesController> logger)
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
                var tmp = f.Type.Split('/').ToList();
                string name = tmp.Last();
                string type = tmp.FirstOrDefault();
                var user = await _userManager.GetUserAsync(HttpContext.User);
                f.UserId = user.Id;
                f.User = user;
                f.Name = f.Name;
                f.Type = f.Type;
                var favorites = db.Favorites.Where(fav => fav.UserId == user.Id).ToList();
                if (favorites.Where(x => x.Type == f.Type).Any() == false)
                {
                    db.Favorites.Add(f);
                    db.SaveChanges();
                    favorites.Add(f);
                }
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