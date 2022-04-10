using DbConnectionClassLib.Parameters;
using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public List<float> CalculateVolumeDataFromCapitalization(List<TIME_SERIES_DAILY_ADJUSTED> datas, DateTime first, DateTime last)
        {
            List<float> list = new List<float>();
            float firstClosePrice = (float)datas.FirstOrDefault().adjusted_close;
            TIME_SERIES_DAILY_ADJUSTED prev = new TIME_SERIES_DAILY_ADJUSTED() { adjusted_close = firstClosePrice };
            foreach (TIME_SERIES_DAILY_ADJUSTED p in datas)
            {
                if (p.timestamp >= first && p.timestamp <= last)
                {
                    float volume = (float)(p.capitalization - prev.capitalization);
                    list.Add(volume);
                }
                p.CopyTo(prev);
            }
            return list;
        }
    }
}
