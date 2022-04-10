using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using static DbConnectionClassLib.Data.DatabaseInstance;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public List<float> getVolumeChangeData(List<TIME_SERIES_DAILY_ADJUSTED> datas, DateTime first, DateTime last)
        {
            List<float> list = new List<float>();
            TIME_SERIES_DAILY_ADJUSTED prev = null;
            foreach (TIME_SERIES_DAILY_ADJUSTED p in datas)
            {
                float vol = (float)p.volume;
                if (prev == null)
                {
                    prev = new TIME_SERIES_DAILY_ADJUSTED();
                    p.CopyTo(prev);
                    if (p.timestamp >= first && p.timestamp <= last)
                        list.Add(vol);
                    continue;
                }
                if (prev.adjusted_close > p.adjusted_close)
                {
                    vol *= -1;
                }
                p.CopyTo(prev);
                if (p.timestamp >= first && p.timestamp <= last)
                    list.Add(vol);
            }
            return list;
        }
    }
}
