using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace WizlinkPluginCloudVision
{
    [WizlinkVisible]
    public class Plugin : PluginBase
    {
        [Description("Performs OCR on the provided image and returns the result")]
        [return: TupleDescription(new string[] { "Data", "Confidence" })]
        public async Task<(string ocrResult, float overallConfidence)> PerformOCR(string imagePath, string apiKey, CancellationToken cancellationToken)
        {
            string requestUri = "https://vision.googleapis.com/v1/images:annotate?key=" + apiKey;
            using (var client = new HttpClient())
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string imageContent = Convert.ToBase64String(imageBytes);

                string jsonRequest = $"{{\"requests\":[{{\"image\":{{\"content\":\"{imageContent}\"}},\"features\":[{{\"type\":\"DOCUMENT_TEXT_DETECTION\"}}]}}]}}";
                StringContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(requestUri, content, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    string myPESEL = "";
                    base.Log($"Log from plugin: {response:StatusCode}");
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<Responses>(jsonResponse);
                    float overallConfidence = responseObject.responses[0].fullTextAnnotation.pages[0].confidence;
                    string peselPATH = responseObject.responses[0].textAnnotations[0].description;
                    peselPATH.Replace("\n", "");
                    int indexPESEL = peselPATH.IndexOf("PESEL 1");
                    if (indexPESEL != -1) { myPESEL = peselPATH.Substring(indexPESEL + 7, 12);
                        base.Log($"Log from plugin: PESEL1 position found");
                    } 
                    return (myPESEL, overallConfidence);
                }
                else
                {
                    throw new Exception($"HTTP status code: {response.StatusCode}");
                }
            }
        }
    }

    public class Responses
    {
        public Response[] responses { get; set; }
    }

    public class Response
    {
        public TextAnnotation[] textAnnotations { get; set; }
        public FullTextAnnotation fullTextAnnotation { get; set; }
    }

    public class TextAnnotation
    {
        public string description { get; set; }
    }

    public class FullTextAnnotation
    {
        public string text { get; set; }
        public Page[] pages { get; set; }
    }

    public class Page
    {
        public float confidence { get; set; }
    }
}
