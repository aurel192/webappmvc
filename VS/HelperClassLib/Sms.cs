using System;
using System.Collections.Generic;
using System.Text;

namespace HelperClassLib
{
    public static class Sms
    {
        public static bool Send(string recipient, string msg)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
            kvpList.Add(new KeyValuePair<string, string>("api_key", Constants.nexmo_apikey));
            kvpList.Add(new KeyValuePair<string, string>("api_secret", Constants.nexmo_api_secret));
            kvpList.Add(new KeyValuePair<string, string>("from", "NEXMO"));
            kvpList.Add(new KeyValuePair<string, string>("to", recipient));
            kvpList.Add(new KeyValuePair<string, string>("text", msg));
            var result = Http.Post("https://rest.nexmo.com/", "sms/json", kvpList).Result;
            return true;
        }
    }
}

/*
{
    "message-count": "1",
    "messages": [{
        "to": "36205202454",
        "message-id": "0D000000AAB1DF89",
        "status": "0",
        "remaining-balance": "11.51910000",
        "message-price": "0.06870000",
        "network": "21601"
    }]
}
*/
