using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WizlinkPluginCloudVision
{
    [WizlinkVisible]
    public class Plugin : PluginBase
    {
        [Description("Performs OCR on the provided image and returns the result")]
        [return: TupleDescription(new string[] { "status", "response" })]
        public async Task<(string status, string response)> PerformOCR(string imagePath, string apiKey, CancellationToken cancellationToken)
        {
            string requestUri = "https://vision.googleapis.com/v1/images:annotate?key=" + apiKey;
            using (var client = new HttpClient())
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string imageContent = Convert.ToBase64String(imageBytes);

                string jsonRequest = $"{{\"requests\":[{{\"image\":{{\"content\":\"{imageContent}\"}},\"features\":[{{\"type\":\"TEXT_DETECTION\"}}],\"imageContext\":{{\"languageHints\":[\"pl-t-i0-handwrit\"]}}}}]}}";
                
                StringContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(requestUri, content, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    base.Log($"Log from plugin: {response:StatusCode}");
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return (response.StatusCode.ToString(), jsonResponse);
                }
                else
                {
                    throw new Exception($"HTTP status code: {response.StatusCode}");
                }
            }
        }
    }

}
