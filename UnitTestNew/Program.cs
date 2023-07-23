// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Text.Json;
using System.Text;

string authorization = "gyNYN0762kVBFHvbEGEQzwpyPmuh8AHV";
HttpClient client;
client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", authorization);
StringContent content = new(JsonSerializer.Serialize(new
{
    smfile = File.ReadAllBytes("C:\\Users\\XianlitiCN\\Pictures\\70060680_p0_master1200.jpg"),
    format = "json"
}), Encoding.UTF8, "multipart/form-data");
HttpResponseMessage r = await client.PostAsync("https://sm.ms/api/v2/upload", content);