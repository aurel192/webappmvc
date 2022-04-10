using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using TicTacTec.TA.Library;
using DbConnectionClassLib.ResponseClasses;

namespace HelperClassLib
{
    public static class TechnicalAnalysis
    {
        public static List<BBandValues> BBands(List<DateTimeDoublePair> points, int period = 10, double devUp = 2, double devDown = 2, Core.MAType MAType = Core.MAType.Sma)
        {
            List<BBandValues> result = new List<BBandValues>();
            try
            {
                int begIdx = -1;
                int numberOfElements = -1;
                double[] outUpper = new double[points.Count + period];
                double[] outMiddle = new double[points.Count + period];
                double[] outLower = new double[points.Count + period];
                var srcArray = (from p in points select p.value).ToArray();
                Core.Bbands(0, points.Count - 1, srcArray, period, devUp, devDown, MAType, out begIdx, out numberOfElements, outUpper, outMiddle, outLower);
                int i = 0;
                foreach (var p in points)
                {
                    BBandValues bbandPoint = new BBandValues() { date = p.date, value = p.value };
                    if (i >= begIdx)
                    {
                        bbandPoint.lower = (float)outLower[i - begIdx];
                        bbandPoint.middle = (float)outMiddle[i - begIdx];
                        bbandPoint.upper = (float)outUpper[i - begIdx];
                    }
                    else
                    {
                        bbandPoint.lower = (float)outLower.FirstOrDefault();
                        bbandPoint.middle = (float)outMiddle.FirstOrDefault();
                        bbandPoint.upper = (float)outUpper.FirstOrDefault();
                    }
                    result.Add(bbandPoint);
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static List<RSIPoint> RSI(List<DateTimeDoublePair> points, out int beginIndex, int period = 14)
        {
            List<RSIPoint> result = new List<RSIPoint>();
            try
            {
                int begIdx = -1;
                int numberOfElements = -1;
                double[] outArray = new double[points.Count + period];
                var srcArray = (from p in points select p.value).ToArray();
                Core.Rsi(0, points.Count - 1, srcArray, period, out begIdx, out numberOfElements, outArray);
                beginIndex = begIdx;
                int i = 0;
                foreach (var p in points)
                {
                    RSIPoint rsiPoint = new RSIPoint() { date = p.date, rsi = 0, x = "", value = p.value };
                    if (i >= begIdx)
                        rsiPoint.rsi = (float)outArray[i - begIdx];
                    else
                        rsiPoint.rsi = 50;
                    result.Add(rsiPoint);
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static List<MACDValues> MACD(List<DateTimeDoublePair> points, out int beginIndex, int fast = 12, int slow = 26, int signal = 9)
        {
            List<MACDValues> result = new List<MACDValues>();
            try
            {
                int begIdx = -1;
                int numberOfElements = -1;
                int size = points.Count + (slow * 2);
                double[] outMACD = new double[size];
                double[] outHist = new double[size];
                double[] outMACDSignal = new double[size];
                var srcArray = (from p in points select p.value).ToArray();
                Core.Macd(0, points.Count - 1, srcArray, fast, slow, signal, out begIdx, out numberOfElements, outMACD, outMACDSignal, outHist);
                beginIndex = begIdx;
                int i = 0;
                foreach (var p in points)
                {
                    MACDValues macdPoint = new MACDValues() { date = p.date, value = p.value };
                    if (i >= begIdx)
                    {
                        macdPoint.macd = (float)outMACD[i - begIdx];
                        macdPoint.hist = (float)outHist[i - begIdx];
                        macdPoint.signal = (float)outMACDSignal[i - begIdx];
                    }
                    result.Add(macdPoint);
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
