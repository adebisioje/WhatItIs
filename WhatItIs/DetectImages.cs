using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WhatItIs
{
    public class DetectImages
    {

        private static readonly VisualFeature[] VisualFeatures = { VisualFeature.Description };


        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        public static async Task<string> DetectImageAsync(Stream stream)
        {
            var response = await MakeAnalysisRequest(stream);
            return response;
        }

        static async Task<string> MakeAnalysisRequest(Stream stream)
        {
            var client = new VisionServiceClient("425b34f8517c404fa4d6b0b1b0957292");
            var result = await client.AnalyzeImageAsync(stream, VisualFeatures);
            return ProcessAnalysisResult(result);

    }
        private static string ProcessAnalysisResult(AnalysisResult result)

        {

            string message = result?.Description?.Captions.FirstOrDefault()?.Text;
            return string.IsNullOrEmpty(message) ?

                        "Couldn't find a caption for this one" :

                        "I think it's " + message;

        }

        static async Task<string> MakeAnalysisRequest2(byte[] byteDat)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "2474b217e55942b69797d2f67422bc76");

            // Request parameters
            queryString["visualFeatures"] = "Categories";
            queryString["details"] = "{string}";
            queryString["language"] = "en";
            var uri = "https://westus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + queryString;

            HttpResponseMessage response;

            // Request body
             byte [] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }
            return await response.Content.ReadAsStringAsync();
        }
    }

    
        

    


}
