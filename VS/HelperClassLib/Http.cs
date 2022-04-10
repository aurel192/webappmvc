using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelperClassLib
{
    public static class Http
    {
        public static async Task<string> Get(string baseAddr, string uri = "")
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(baseAddr + uri);
                //HttpResponseMessage response = client.GetAsync(baseAddr + uri).Result;
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                int start = result.IndexOf("<JSON>");
                int end = result.IndexOf("</JSON>");
                if (start > 0 && end > 0)
                    return result.Substring(start + 6, end - start - 6);
                else
                    return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> GetBytes(string baseAddr, string uri = "")
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(baseAddr + uri);
                HttpContent content = response.Content;
                var result = await content.ReadAsByteArrayAsync();
                string stringResult = System.Text.Encoding.Default.GetString(result);
                return stringResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> Post(string baseAddr, string uri, List<KeyValuePair<string, string>> kvpList)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddr);
                    FormUrlEncodedContent content = new FormUrlEncodedContent(kvpList);
                    HttpResponseMessage response = await client.PostAsync(uri, content);
                    //string resultContent = await response.Content.ReadAsStringAsync();
                    var resultContentByte = await response.Content.ReadAsByteArrayAsync();
                    //string resultContent = Encoding.Default.GetString(resultContentByte);
                    string resultContent = Encoding.UTF7.GetString(resultContentByte);
                    int start = resultContent.IndexOf("<JSON>");
                    int end = resultContent.IndexOf("</JSON>");
                    if (start > 0 && end > 0)
                        return resultContent.Substring(start + 6, end - start - 6);
                    else
                        return resultContent;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
