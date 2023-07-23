using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        internal static string UploadImageAsync(FileInfo imageInfo)
        {
            HttpContent content = new StreamContent(imageInfo.OpenRead());
            var mp = new MultipartFormDataContent();
            mp.Add(content, "smfile", imageInfo.Name);
            HttpResponseMessage r = client.PostAsync("https://smms.app/api/v2/upload", mp).Result;
            string rstr = r.Content.ReadAsStringAsync().Result;
            JsonDocument doc = JsonDocument.Parse(rstr);
            bool success = doc.RootElement.GetProperty("success").GetBoolean();
            if (success)
            {
                return doc.RootElement.GetProperty("data").GetProperty("url").GetString();
            }
            else
            {
                return doc.RootElement.GetProperty("images").GetString();
            }
        }
    }
}
