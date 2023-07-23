using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal static class SmMsHelper
    {
        private const string authorization = "gyNYN0762kVBFHvbEGEQzwpyPmuh8AHV";

        private static HttpClient client;

        static SmMsHelper()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", authorization);
        }

        internal async static void UploadImageAsync(string path)
        {
            StringContent content = new(JsonSerializer.Serialize(new
            {
                smfile = File.ReadAllBytes(path),
                format = "json"
            }), Encoding.UTF8, "multipart/form-data");
            HttpResponseMessage r = await client.PostAsync("https://sm.ms/api/v2/upload", content);
        }
    }
}
