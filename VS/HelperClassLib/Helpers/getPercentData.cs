using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public List<double> getPercentData(List<double> datas)
        {
            List<double> list = new List<double>();
            double first = datas.Where(d => d != 0).FirstOrDefault();
            foreach (var p in datas)
            {
                list.Add(Math.Round((100 * p / first), 2));
            }
            return list;
        }
    }
}
