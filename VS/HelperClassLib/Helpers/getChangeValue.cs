using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public List<double> GetChangeValue(List<double> datas)
        {
            List<double> list = new List<double>();
            double prev = 0;
            foreach (var p in datas)
            {
                double change = 0;
                if (prev != 0)
                    change = Math.Round((100 * (p - prev) / prev), 2);
                list.Add(change);
                prev = p;
            }
            return list;
        }

        public List<double> GetChangeValue(List<OHLC> datas, bool beforeOpening = false)
        {
            List<double> list = new List<double>();
            OHLC prev = null;
            foreach (OHLC p in datas)
            {
                double change = 0;
                if (prev != null)
                {
                    if (beforeOpening)
                        change = Math.Round((100 * (p.open - prev.close) / prev.close), 2);
                    else
                        change = Math.Round((100 * (p.close - prev.close) / prev.close), 2);
                }
                list.Add(change);
                prev = p;
            }
            return list;
        }
    }
}
