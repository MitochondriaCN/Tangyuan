using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal static class VersionControlHelper
    {
        static HttpClient client;
        const string apikey = "133d8c604b4d0772723a007a9ad213f7";
        const string appkey = "862082e246e4c9d4615cc9919262e7c7";
        static VersionControlHelper()
        {
            client = new HttpClient();
            
        }

        /// <summary>
        ///  检查是否有新版本
        /// </summary>
        /// <returns></returns>
        internal async static Task<bool> IsANewerVersionReleasedAsync()
        {
            List<KeyValuePair<string, string>> args = new()
            {
                new("_api_key",apikey),
                new("appKey",appkey),
                new("buildVersion",AppInfo.Current.BuildString)
            };
            FormUrlEncodedContent content = new(args);
            HttpResponseMessage resp = await client.PostAsync("https://www.pgyer.com/apiv2/app/check", content);
            JsonDocument respdoc = JsonDocument.Parse(resp.Content.ReadAsStringAsync().Result);
            return respdoc.RootElement.GetProperty("data").GetProperty("buildHaveNewVersion").GetBoolean();
        }

        /// <summary>
        /// 下载最新版本
        /// </summary>
        /// <returns>下载路径</returns>
        internal async static Task<string> DownloadNewestVersionAsync()
        {
            HttpResponseMessage resp = await client.GetAsync("https://www.pgyer.com/apiv2/app/install?_api_key=" + apikey + "&appKey=" + appkey);
            var stream = resp.Content.ReadAsStream();
            using (var filestream = new FileStream(Path.Combine(FileSystem.Current.CacheDirectory, "newversion.apk"), FileMode.Create))
            {
                stream.CopyTo(filestream);
            }
            return Path.Combine(FileSystem.Current.CacheDirectory, "newversion.apk");
        }
    }
}
