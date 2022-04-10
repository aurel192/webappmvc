using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionClassLib.Data;
using DbConnectionClassLib.ResponseClasses;
using DbConnectionClassLib.Tables;
using HelperClassLib;
using HelperClassLib.AlphaVantage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMVC.Services;
using static HelperClassLib.Helpers.HelperClass;

namespace WebAppMVC.ApiControllers
{
    [Produces("application/json")]
    [Route("api/SearchSymbol")]
    public class SearchSymbol : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public SearchSymbol(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<AV_SYMBOL_SEARCH_RESPONSE>> Get(string keywords = "")
        {
            try
            {
                List<AV_SYMBOL_SEARCH_RESPONSE> fromDb = new List<AV_SYMBOL_SEARCH_RESPONSE>();
                List<AV_SYMBOL_SEARCH_RESPONSE> fromAVapi = new List<AV_SYMBOL_SEARCH_RESPONSE>();
                if (keywords.Length >= 2)
                {
                    fromDb = db.av_time_series_daily_adjusted
                               .Where(a => a.symbol.Contains(keywords) || a.name.Contains(keywords))
                               .Select(a => new AV_SYMBOL_SEARCH_RESPONSE
                               {
                                   name = a.name,
                                   symbol = a.symbol,
                               }).ToList();
                }
                try
                {
                    DateTime lastSearch = (from l in db.db_log
                                           where l.TimeStamp > DateTime.Now.AddDays(-3) &&
                                           l.Key == "/query?function=SYMBOL_SEARCH&keywords=" + keywords + "&datatype=csv"
                                           orderby l.Id descending
                                           select l.TimeStamp).FirstOrDefault();
                   
                    if (lastSearch.IsValidDate() == false)
                    {
                        AlphaVantageClient avc = new AlphaVantageClient(new AlphaVantageSymbolSearchParameters(keywords));
                        fromAVapi = avc.GetDataSymbolSearch();
                    }
                }
                catch {
                    // TODO: Letarolni mit nem sikerult lekerni az api-tol, majd kesobb ujra proba
                }
                List<AV_SYMBOL_SEARCH_RESPONSE> results = fromAVapi.Union(fromDb).ToList().DistinctBy(r => r.symbol).ToList().OrderBy(r => r.name).ToList();
                return results;
            }
            catch (Exception ex)
            {
                ApplicationUser user = null;
                try { user = await _userManager.GetUserAsync(HttpContext.User); }
                catch { }
                Helper.LogException(ex, Request, user);
                throw ex;
            }
        }
    }
}