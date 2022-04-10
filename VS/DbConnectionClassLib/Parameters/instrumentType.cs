using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionClassLib.Parameters
{
    public enum instrumentType
    {
        OtpFunds = 1,
        Equities = 2,
        Indexes = 3,
        Forex = 4,
        Crypto = 5,
        HungarianEquities = 6,
        HungarianEquitiesIntraday = 66,
        HungarianMutualFunds = 7,
        HungarianEquitiesBET = 8,
        Commodities = 9,
        HungarianMaxIndexes = 10
    }

    public enum CandleInterval
    {
        Min_1 = 1,
        Min_5 = 2,
        Min_15 = 3,
        Min_30 = 4,
        Min_60 = 5,
        Daily = 7,
        Weekly = 8,
        Monthly = 9,
        Yearly = 10
    }
}
