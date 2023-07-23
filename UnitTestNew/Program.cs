// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Text.Json;
using System.Text;

string authorization = "gyNYN0762kVBFHvbEGEQzwpyPmuh8AHV";
HttpClient client;
client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", authorization);
HttpContent content = new StreamContent(new FileInfo("C:\\Users\\XianlitiCN\\Pictures\\get.jpg").OpenRead());
var mp = new MultipartFormDataContent();
mp.Add(content, "smfile", "123.jpg");
HttpResponseMessage r = await client.PostAsync("https://sm.ms/api/v2/upload", mp);
Console.WriteLine(await r.Content.ReadAsStringAsync());
Console.ReadLine();