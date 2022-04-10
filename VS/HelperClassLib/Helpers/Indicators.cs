using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using TicTacTec.TA.Library;
using DbConnectionClassLib.ResponseClasses;

namespace HelperClassLib.Helpers
{
    public static class Indicators
    {
        public static List<DateTimeDoublePair> MA(List<DateTimeDoublePair> points, out int beginIndex, int period)
        {
            List<DateTimeDoublePair> result = new List<DateTimeDoublePair>();
            double[] outArray = new double[points.Count + period];
            beginIndex = -1;
            int begIdx = -1;
            int numberOfElements = -1;
            try
            {
                var srcArray = (from p in points select p.value).ToArray();
                Core.MovingAverage(0, points.Count - 1, srcArray, period, Core.MAType.Sma, out begIdx, out numberOfElements, outArray);
                beginIndex = begIdx;
                int i = 0;
                foreach (var p in points)
                {
                    DateTimeDoublePair maPoint = new DateTimeDoublePair() { date = p.date };
                    if (i >= begIdx)
                        maPoint.value = (float)outArray[i - begIdx];
                    else
                        maPoint.value = p.value;
                    result.Add(maPoint);
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static List<OHLC> GetHeikinAshiOhlcChart(List<OHLC> points)
        {
            List<OHLC> HApoints = new List<OHLC>();
            try
            {
                if (!points.Any())
                    throw new Exception("ERROR GetHeikinAshiOhlcChart No OHLC points found");
                OHLC prev = new OHLC();
                points.FirstOrDefault().CopyTo(prev);
                
                foreach (OHLC p in points)
                {
                    OHLC haCandle = new OHLC();
                    haCandle.close = Math.Round((p.open + p.high + p.low + p.close) / 4, 2);
                    haCandle.open = Math.Round((prev.open + prev.close) / 2, 2);
                    haCandle.high = (new List<double> { p.high, haCandle.open, haCandle.close }).Max();
                    haCandle.low = (new List<double> { p.low, haCandle.open, haCandle.close }).Min();
                    HApoints.Add(haCandle);
                    p.CopyTo(prev);
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return HApoints;
        }
    }
}