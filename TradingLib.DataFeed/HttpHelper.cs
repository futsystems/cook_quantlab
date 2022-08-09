using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TradingLib.DataFeed
{
    public class HttpHelper
    {
        public static T Post<T>(string url, object data, string contentType = "application/json",JsonSerializerSettings jsonSetting = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            byte[] postBytes = null;
            if (jsonSetting == null)
            {
                postBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            }
            else
            {
                postBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, jsonSetting));
            }
            request.ContentType = contentType;

            string result = string.Empty;
            return Task.Run(async () =>
            {
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    using (var response = await request.GetResponseAsync())
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            result = reader.ReadToEnd();
                            return JsonConvert.DeserializeObject<T>(result);
                        }
                    }
                }
            }).Result;
        }

        public static T Get<T>(string uri, int timeOut = 10, bool checkValidationResult = false)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            return Task.Run(async () =>
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                        }
                    }
                }
            }).Result;
        }
    }
}
