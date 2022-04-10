using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using HelperClassLib;
using static DbConnectionClassLib.Data.DatabaseInstance;
using static HelperClassLib.Helpers.HelperClass;
using DbConnectionClassLib.Data;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public List<ListViewListItem> GetInstrumentListViewListItems(ApplicationUser applicationUser = null)
        {
            List<ListViewListItem> instruments = new List<ListViewListItem>();


            List<string> fundtypes = new List<string>() {
                        "ACCESS",
                        "AEGON",
                        "Aberdeen Global",
                        "Accorde",
                        "Aegon",
                        "Allianz",
                        "Amundi",
                        "BF Money",
                        "BGF",
                        "Budapest",
                        "CIB",
                        "Concorde",
                        "Credit Suisse",
                        "DIALÓG",
                        "Diófa",
                        "EQUILOR",
                        "ERSTE",
                        "ESPA",
                        "Eurizon",
                        "Franklin",
                        "Generali",
                        "ING (L)",
                        "ING",
                        "JPM",
                        "K&H",
                        "MKB",
                        "Magyar Posta",
                        "OTP",
                        "QUANTIS",
                        "Raiffeisen",
                        "Templeton",
            };

            List<string> alphabet = new List<string>() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J","K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

            try
            {
                var available = new List<int> { 2, 3, /*4, 5,*/ 6, /*7, 9, 10 */};
                instruments = (from it in Db.InstrumentTypes
                               where available.Contains(it.Number)
                               select new ListViewListItem
                               {
                                   id = it.Name,
                                   text = it.Name,
                                   child = null,
                                   instumentType = it.Number
                               }).ToList();

                if (applicationUser != null)
                {
                    var favorites = Db.Favorites.Where(fav => fav.UserId == applicationUser.Id).ToList();
                    if (favorites.Any())
                    {
                        var favItems = new ListViewListItem() { text = "Favorites", id = "Favorites", instumentType = 100, child = new List<ListViewListItem>() };
                        foreach (var f in favorites)
                        {
                            var tmp = f.Type.Split('/').ToList();
                            var type = tmp.FirstOrDefault();
                            favItems.child.Add(new ListViewListItem() { text = f.Name, id = f.Type, instumentType = (int)getInstrumentTypeByTypeName(type) });
                        }
                        instruments.Insert(0, favItems);
                    }
                }

                foreach (var t in instruments)
                {
                    if (t.text != "Favorites")
                        t.child = new List<ListViewListItem>();
                    switch ((instrumentType)t.instumentType)
                    {
                        case instrumentType.Indexes:
                            t.child.Add(new ListViewListItem { text = "Dow Jones Industrial Avarage", id = "Equities/^DJI", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "S&P 500", id = "Equities/^GSPC", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "NASDAQ", id = "Equities/^IXIC", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "	NYSE COMPOSITE (DJ)", id = "Equities/^NYA", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "NYSE AMEX COMPOSITE INDEX", id = "Equities/^XAX", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "BATS 1000 Index", id = "Equities/^BATSK", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "Russell 2000", id = "Equities/^RUT", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "VIX", id = "Equities/^VIX", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "FTSE 100", id = "Equities/^FTSE", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "DAX PERFORMANCE-INDEX", id = "Equities/^GDAXI", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "CAC 40", id = "Equities/^FCHI", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "ESTX 50 PR.EUR", id = "Equities/^STOXX50E", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "EURONEXT 100", id = "Equities/^N100", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "BEL 20", id = "Equities/^BFX", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "MICEX Index", id = "Equities/^MICEXINDEXCF.ME", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "Nikkei 225", id = "Equities/^N225", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "HANG SENG INDEX", id = "Equities/^HSI", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "SSE Composite Index", id = "Equities/^000001.SS", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "STI Index", id = "Equities/^STI", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "S&P/ASX 200", id = "Equities/^AXJO", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "ALL ORDINARIES", id = "Equities/^AORD", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "S&P BSE SENSEX", id = "Equities/^BSESN", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "Jakarta Composite Index", id = "Equities/^JKSE", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "FTSE Bursa Malaysia KLCI", id = "Equities/^KLSE", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "S&P/NZX 50 INDEX GROSS", id = "Equities/^NZ50", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "KOSPI Composite Index", id = "Equities/^KS11", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "TSEC weighted index", id = "Equities/^TWII", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "S&P/TSX Composite index", id = "Equities/^GSPTSE", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "IBOVESPA", id = "Equities/^BVSP", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "IPC MEXICO", id = "Equities/^MXX", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "IPSA SANTIAGO DE CHILE", id = "Equities/^IPSA", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "MERVAL", id = "Equities/^MERV", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "TA-125", id = "Equities/^TA100", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "EGX 30 Price Return Index", id = "Equities/^CASE30", instumentType = t.instumentType });
                            t.child.Add(new ListViewListItem { text = "FTSE/JSE TOP 40 USD", id = "Equities/^JN0U.FGI", instumentType = t.instumentType });
                            t.child = t.child.OrderBy(x => x.text).ToList();
                            break;

                        case instrumentType.Equities:
                            var stocks = Db.av_time_series_daily_adjusted.Select(s => new ListViewListItem { text = s.name, id = "Equities/" + s.symbol, instumentType = t.instumentType }).ToList();
                            foreach (var stock in stocks) {
                                stock.text = stock.text.UpperCaseFirst();
                            }
                            foreach (var letter in alphabet)
                            {
                                var StocksBelonging = stocks.Where(f => f.text.StartsWith(letter)).OrderBy(f => f.text).ToList();
                                var child = new ListViewListItem()
                                {
                                    text = letter,
                                    id = "Equities/" + letter,
                                    child = StocksBelonging,
                                    instumentType = t.instumentType
                                };
                                t.child.Add(child);
                                stocks = stocks.Except(StocksBelonging).ToList();
                            }
                            var others = new ListViewListItem()
                            {
                                text = "Others",
                                id = "Equities/" + "Others",
                                child = stocks,
                                instumentType = t.instumentType
                            };
                            t.child.Add(others);
                            break;
                        case instrumentType.HungarianEquities:
                            t.child = Db.portfolio_hunstock.Select(phs => new ListViewListItem { text = phs.name, id = "Hungarian Equities/" + phs.name, instumentType = t.instumentType }).OrderBy(phs => phs.text).ToList();
                            break;
                        case instrumentType.HungarianMutualFunds:
                            var funds = Db.portfolio_hunfund.Select(phf => new ListViewListItem { text = phf.name, id = "Hungarian Mutual Funds/" + phf.name, instumentType = t.instumentType }).ToList();                            
                            foreach (var ft in fundtypes)
                            {
                                var FundsBelonging = funds.Where(f => f.text.StartsWith(ft)).OrderBy(f => f.text).ToList();
                                var child = new ListViewListItem() {
                                    text = ft,
                                    id = "Hungarian Mutual Funds/" + ft,
                                    child = FundsBelonging,
                                    instumentType = t.instumentType
                                };
                                t.child.Add(child);
                                funds = funds.Except(FundsBelonging).ToList();
                            }
                            others = new ListViewListItem()
                            {
                                text = "Others",
                                id = "Hungarian Mutual Funds/" + "Others",
                                child = funds,
                                instumentType = t.instumentType
                            };
                            t.child.Add(others);
                            break;
                        case instrumentType.HungarianMaxIndexes:
                            t.child = Db.portfolio_allampapir.Select(pha => new ListViewListItem { text = pha.name, id = "Hungarian MAX Indexes/" + pha.name, instumentType = t.instumentType }).ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return instruments;
        }
    }
}
