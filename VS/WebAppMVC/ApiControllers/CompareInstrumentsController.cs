using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbConnectionClassLib.Data;
using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using HelperClassLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppMVC.Services;
using static HelperClassLib.Helpers.HelperClass;

namespace WebAppMVC.ApiControllers
{
    [Produces("application/json")]
    [Route("api/CompareInstruments")]
    public class CompareInstrumentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext db;
        List<string> colors = new List<string>() { "#e6194b", "#3cb44b", "#ffe119", "#0082c8", "#f58231", "#911eb4", "#46f0f0", "#f032e6", "#d2f53c", "#fabebe", "#008080", "#e6beff", "#aa6e28", "#fffac8", "#800000", "#aaffc3", "#808000", "#ffd8b1", "#000080", "#808080" };

        public CompareInstrumentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<CompareInstrumentsController> logger)
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost]
        public async Task<CompareResponse> Post(CompareInstrumentsParam parameter)
        {
            CompareResponse response = new CompareResponse();
            try
            {
                DateTime datefrom = parameter.startDateStr.ToDate(DateTime.MinValue);
                DateTime dateto = parameter.endDateStr.ToDate(DateTime.MaxValue);
                datefrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day, 0, 0, 0);
                dateto = new DateTime(dateto.Year, dateto.Month, dateto.Day, 23, 59, 59);
                response.DateTimeData.min = DateTime.MaxValue;
                response.DateTimeData.max = DateTime.MinValue;

                List<TSDAListItem> listIncomplete = new List<TSDAListItem>();

                var av_list = new List<FormElement>();
                av_list = parameter.formData.Where(p => p.type == 2)
                                                .Select(p => new FormElement
                                                {
                                                    instrumentFullPath = p.instrumentFullPath,
                                                    instrumentName = p.instrumentName,
                                                    stock = p.stock,
                                                    text = p.text,
                                                    type = p.type,
                                                    symbol = p.instrumentFullPath.Replace("Equities/", "")
                                                }).ToList();

                var orderedList = db.av_time_series_daily_adjusted.Where(p => av_list.Select(s => s.symbol).Contains(p.symbol)).OrderBy(p => p.lastupdate).ToList();
                av_list = new List<FormElement>();
                foreach (var l in orderedList)
                {
                    var fe = parameter.formData.Where(f => f.instrumentFullPath == "Equities/" + l.symbol).FirstOrDefault();
                    av_list.Add(fe);
                }
                av_list.AddRange(parameter.formData.Where(p => p.type != 2).ToList());


                foreach (FormElement s in av_list)
                {
                    var tmp = s.instrumentFullPath.Split('/').ToList();
                    var instrument = tmp.Last();
                    var name = tmp.FirstOrDefault();

                    if (string.IsNullOrEmpty(name)) continue;
                    TSDAListItem listItem = new TSDAListItem() { name = s.instrumentName };
                    var datas = await Helper.GetInstrumentDataBetweenDates(datefrom, dateto, (instrumentType)s.type, instrument, s.instrumentName);
                    listItem.datas = datas.Select(d => new TIME_SERIES_DAILY_ADJUSTED_MIN
                    {
                        timestamp = d.timestamp,
                        adjusted_close = d.adjusted_close
                    }).ToList();
                    List<DateTime> newDates = listItem.datas.Select(d => d.timestamp).ToList();
                    response.DateTimeData.points.AddRange(newDates.Except(response.DateTimeData.points));
                    listIncomplete.Add(listItem);

                    response.RankingData.Add(Ranking.CreateRanking(datas, s.stock, s.instrumentName, (instrumentType)s.type));
                }
                response.DateTimeData.points = response.DateTimeData.points.OrderBy(d => d).ToList();

                List<TSDAListItem> listComplete = new List<TSDAListItem>();
                foreach (TSDAListItem item in listIncomplete)
                {
                    TSDAListItem cItem = new TSDAListItem() { name = item.name, datas = new List<TIME_SERIES_DAILY_ADJUSTED_MIN>() };
                    foreach (DateTime date in response.DateTimeData.points)
                    {
                        cItem.datas.Add(getNearestPoint(date, item.datas));
                    }
                    listComplete.Add(cItem);
                    item.datas = null;
                }

                response.DateTimeData.min = response.DateFrom = response.DateTimeData.points.Min();
                response.DateTimeData.max = response.DateTo = response.DateTimeData.points.Max();
                response.min = double.MaxValue;
                response.max = double.MinValue;
                int cntr = 0;
                foreach (TSDAListItem item in listComplete)
                {
                    PercentData pd = new PercentData(item.name, "line", colors.ElementAt(cntr++))
                    {
                        points = Helper.getPercentData(item.datas.Select(d => d.adjusted_close).ToList())
                    };
                    //PercentData pd = AddPercantageDataColored(item);
                    pd.min = pd.points.Min();
                    pd.max = pd.points.Max();
                    if (pd.min < response.min)
                        response.min = Math.Floor(pd.min);
                    if (pd.max > response.max)
                        response.max = Math.Ceiling(pd.max);
                    response.PercentData.Add(pd);
                }
            }
            catch (Exception ex)
            {
                ApplicationUser user = null;
                try
                {
                    user = await _userManager.GetUserAsync(HttpContext.User);
                }
                catch (Exception) { }
                Helper.LogException(ex, Request, user);
                response.error = ex.Message;
            }
            return response;
        }

        private PercentData AddPercantageDataColored(TSDAListItem item)
        {
            string color = "#4287f5";
            if (item.name.Contains("OTP"))
            {
                color = "3cb44b";
                colors = colors.Where(c => c != color).ToList();
            }
            //else if (item.name.Contains("RICHTER"))
            //{
            //    color = "";
            //}
            else if (item.name.Contains("MTEL"))
            {
                color = "e6194b";
                colors = colors.Where(c => c != color).ToList();
            }
            //else if (item.name.Contains("MOL"))
            //{
            //    color = "";
            //}
            else if (item.name.Contains("(CAT)"))
            {
                color = "ffe119";
                colors = colors.Where(c => c != color).ToList();
            }
            //else if (item.name.Contains("(MMM)"))
            //{
            //    color = "";
            //}
            //else if (item.name.Contains("(PEP)"))
            //{
            //    color = "";
            //}
            else
            {
                color = colors.FirstOrDefault();
                colors = colors.Where(c => c != color).ToList();
            }
            var pd = new PercentData(item.name, "line", color)
            {
                points = Helper.getPercentData(item.datas.Select(d => d.adjusted_close).ToList())
            };
            return pd;
        }

        //private string GetRandomColor()
        //{
        //    if (colors.Any())
        //    {
        //        Random r = new Random(colors.Count);
        //        var random = r.Next();
        //        return colors.ElementAt(random - 1);
        //    }
        //    return "#4287f5";
        //}

        private static TIME_SERIES_DAILY_ADJUSTED_MIN getNearestPoint(DateTime date, List<TIME_SERIES_DAILY_ADJUSTED_MIN> datas)
        {
            TIME_SERIES_DAILY_ADJUSTED_MIN ret = new TIME_SERIES_DAILY_ADJUSTED_MIN() { timestamp = date };
            try
            {
                var value = datas.Where(d => d.timestamp == date).FirstOrDefault();
                if (value != null)
                {
                    ret.adjusted_close = value.adjusted_close;
                }
                else
                {
                    var nd = findNearestDateWhenDataAvailable(date, datas.Select(d => d.timestamp).ToList());
                    var ndValue = datas.Where(d => d.timestamp == nd).FirstOrDefault();
                    if (ndValue != null)
                    {
                        ret.adjusted_close = ndValue.adjusted_close;
                    }
                    else
                    {
                        ret.adjusted_close = 0;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ret;
        }

        private static DateTime findNearestDateWhenDataAvailable(DateTime targetDate, List<DateTime> dates)
        {
            DateTime closestDate = new DateTime(1900, 1, 1);
            try
            {

                long min = long.MaxValue;
                foreach (DateTime date in dates)
                {
                    if (Math.Abs(date.Ticks - targetDate.Ticks) < min)
                    {
                        min = Math.Abs(date.Ticks - targetDate.Ticks);
                        closestDate = date;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return closestDate;
        }
    }
}