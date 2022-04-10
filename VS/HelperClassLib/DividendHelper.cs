using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelperClassLib
{
    public static class DividendHelper
    {
        public static List<Dividend> GetDividends(List<TIME_SERIES_DAILY_ADJUSTED> olhcPoints)
        {
            var dividendPoints = new List<Dividend>();
            int cntr = 0;
            try
            {
                foreach (var p in olhcPoints)
                {
                    if (p.dividend_amount > 0)
                    {
                        dividendPoints.Add(new Dividend() { pos = cntr, value = p.high, date = p.timestamp.ToString("yyyy-MM-dd"), dividendValue = p.dividend_amount });
                    }
                    cntr++;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return dividendPoints;
        }
    }
}
