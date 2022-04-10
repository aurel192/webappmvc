using DbConnectionClassLib.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        public instrumentType getInstrumentTypeByTypeName(string instrtype)
        {
            switch (instrtype)
            {
                case "Otp Funds":
                    return instrumentType.OtpFunds;
                case "Equities":
                    return instrumentType.Equities;
                case "Indexes":
                    return instrumentType.Indexes;
                case "Forex":
                    return instrumentType.Forex;
                case "Crypto":
                    return instrumentType.Crypto;
                case "Hungarian Equities":
                    return instrumentType.HungarianEquities;
                case "Hungarian Mutual Funds":
                    return instrumentType.HungarianMutualFunds;
                case "Hungarian Equities (BÉT)":
                    return instrumentType.HungarianEquitiesBET;
                case "Commodities":
                    return instrumentType.Commodities;
                case "Hungarian MAX Indexes":
                    return instrumentType.HungarianMaxIndexes;
            }

            return instrumentType.Indexes;
        }
    }
}
