using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;
using System.Threading.Tasks;

namespace Core
{

    public class Recognizer
    {
        private String JSON;

        public String getJSON(String filePath)
        {
            MakeRequest(filePath);
            return this.JSON;
        }

        private async void MakeRequest(String filePath)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0a88b606aa3d4ac69c54a6890ad09984");

            var uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?" + queryString;

            HttpResponseMessage response;

            byte[] byteData = System.IO.File.ReadAllBytes(filePath); // Add @ if everything goes wrong

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                
                this.JSON = JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result);


                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                //System.IO.File.WriteAllText(@"D:\Programare\Hack Cambridge\Api Microsoft cognitive\json.txt", response.Content.ReadAsStringAsync().Result);
            }

        }
    }
}
