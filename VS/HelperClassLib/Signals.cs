using DbConnectionClassLib.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperClassLib
{
    public static class Signals
    {
        public static List<Signal> GetRSISignals(List<RSIPoint> rsiPoints)
        {
            var RsiSignals = new List<Signal>();
            RSIPoint prev = new RSIPoint();
            int cntr = 0;
            try
            {
                foreach (var p in rsiPoints)
                {
                    if (cntr > 0)
                    {
                        // IF RSI BECOMES < 30 OVERSOLD - BUY SIGNAL
                        if (p.rsi <= 30 && !(prev.rsi <= 30))
                        {
                            RsiSignals.Add(new Signal() { date = p.date, pos = cntr, value = p.value, type = "BUY", comment = "(RSI Cross Down 30)", signalValue = p.rsi });
                        }
                        // IF RSI BECOMES > 70 OVERBOUGHT - SELL SIGNAL
                        if (p.rsi >= 70 && !(prev.rsi >= 70))
                        {
                            RsiSignals.Add(new Signal() { date = p.date, pos = cntr, value = p.value, type = "SELL", comment = "(RSI Cross Up 70)", signalValue = p.rsi });
                        }
                    }
                    cntr++;
                    p.CopyTo(prev);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RsiSignals;
        }

        public static List<Signal> GetMACDSignals(List<MACDValues> macdPoints, int lookBack = 6)
        {
            List<Signal> AllMacdSignals = new List<Signal>();
            List<Signal> MacdSignals = new List<Signal>();
            List<IntFloatPair> MacdSignalIntegrals = new List<IntFloatPair>();
            MACDPoint prev = new MACDPoint();
            int cntr = 0;
            try
            {
                foreach (var p in macdPoints)
                {
                    if (cntr > 0)
                    {
                        // IF MACD BECOMES > SIGNAL BUY
                        if (p.macd >= p.signal && !(prev.macd >= prev.signal)) // BUY
                        {
                            AllMacdSignals.Add(new Signal() { date = p.date, pos = cntr, value = p.value, type = "BUY", comment = "(MACD Cross Up)", signalValue = p.macd });
                            float integral = IntegralBeforeMacdCrossing(macdPoints.Select(m => m.hist).ToList(), cntr, lookBack);
                            MacdSignalIntegrals.Add(new IntFloatPair { index = cntr, value = integral });
                        }
                        // IF MACD BECOMES < SIGNAL SELL
                        else if (p.macd <= p.signal && !(prev.macd <= prev.signal)) // SELL
                        {
                            AllMacdSignals.Add(new Signal() { date = p.date, pos = cntr, value = p.value, type = "SELL", comment = "(MACD Cross Down)", signalValue = p.macd });
                            float integral = IntegralBeforeMacdCrossing(macdPoints.Select(m => m.hist).ToList(), cntr, lookBack);
                            MacdSignalIntegrals.Add(new IntFloatPair { index = cntr, value = integral });
                        }
                    }
                    cntr++;
                    p.CopyTo(prev);
                }
                if (MacdSignalIntegrals.Any()) // Igy kevesebb Signal jon MACD-bol
                {
                    float minIntegral = MacdSignalIntegrals.Select(v => v.value).Average();
                    minIntegral = minIntegral * (float)0.6;
                    List<int> ValidIndexes = MacdSignalIntegrals.Where(i => i.value >= minIntegral).Select(i => i.index).ToList();
                    MacdSignals = AllMacdSignals.Where(m => ValidIndexes.Contains(m.pos)).ToList();
                }
            }
            catch (Exception ex)
            {
                HelperClassLib.Helpers.HelperClass.Helper.LogException(ex);
            }
            return MacdSignals;
        }

        private static float IntegralBeforeMacdCrossing(List<float> macdHistogramPoints, int pos, int lookBack = 6)
        {
            try
            {
                float integral = 0;
                for (int i = pos - 1; i > pos - lookBack && i > 0; i--)
                {
                    integral += macdHistogramPoints.ElementAt(i);
                }
                return Math.Abs(integral);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PositionsData CalculateProfit(List<Signal> signals)
        {
            PositionsData positions = new PositionsData();
            foreach (var s in signals)
            {
                switch (s.type)
                {
                    case "BUY":
                        positions.openPositions.Add(new OpenPosition { opened = s.date, openPrice = s.value });
                        break;
                    case "SELL":
                        foreach (OpenPosition op in positions.openPositions)
                        {
                            double profit = s.value - op.openPrice;
                            positions.sumProfit += profit;
                            positions.closedPositions.Add(new ClosedPosition { opened = op.opened, openPrice = op.openPrice, closed = s.date, interval = (s.date - op.opened).Days, profit = profit, closePrice = s.value, profitPercent = (profit / op.openPrice) * 100 });
                        }
                        positions.openPositions = new List<OpenPosition>();
                        break;
                }
            }
            return positions;
        }
    }
}
