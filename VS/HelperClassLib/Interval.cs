using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HelperClassLib
{
    public class IntervalResults
    {
        public double value { get; set; }
        public double numberOfValuesBetween { get; set; }
    }

    public static class Interval
    {
        public static Tuple<double, double, double> Calculate(double min, double max, int desiredNum = 10)
        {
            List<IntervalResults> results = new List<IntervalResults>();
            double value = 0.000000001;
            double diff = (max - min);
            if (diff == 0)
            {
                min = min * 0.9;
                max = max * 1.1;
                diff = (max - min);
            }
            while (value < 1000000000)
            {
                var numberOfValuesBetween = diff / value;
                results.Add(new IntervalResults { value = value, numberOfValuesBetween = numberOfValuesBetween });
                value *= 10;
            }
            var minDistance = results.Min(n => Math.Abs(desiredNum - n.numberOfValuesBetween));
            var interval = results.First(n => Math.Abs(desiredNum - n.numberOfValuesBetween) == minDistance);
            var low = double.MaxValue;
            var high = double.MinValue;
            var to = Math.Ceiling((max / interval.value));
            for (int i = 0; i <= to; i++)
            {
                var step = i * interval.value;
                if (min >= step)
                {
                    low = step;
                }
                if (max <= step)
                {
                    high = step;
                }
            }
            var res = new Tuple<double, double, double>(low, high, interval.value);
            return res;
        }
    }
}
