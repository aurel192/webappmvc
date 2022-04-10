using System;
using System.Collections.Generic;
using System.Text;

namespace HelperClassLib
{
    public class Constants
    {
        //public const string host = "http://10.0.0.10";
#if USEDOCKERMYSQL
        public const string logpath = "";
#else
        public const string logpath = @"c:\--CODE--\webappmvc\log\";
        //public const string logpath = @"c:\shared\log\";
#endif
        public const string host = "http://ftm.collectioninventory.com";
        public const string psw = "qwertz123456asd";
        public const string AVKey = "7JXQ";
        public const string Forge1Key = "SVSD1XkztSP9ewtEuH06KlvnsoFWIjbc";
        public const string currencylayerKey = "74c88a28928b26628ab58787e0a32332";

        public const string nexmo_apikey = "f27c3023";
        public const string nexmo_api_secret = "0VE2duBMQsgeHnj3";
    }
}
