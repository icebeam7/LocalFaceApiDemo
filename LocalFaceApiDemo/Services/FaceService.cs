using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using LocalFaceApiDemo.Models;
using LocalFaceApiDemo.Helpers;

using Newtonsoft.Json;
using Plugin.Media.Abstractions;

namespace LocalFaceApiDemo.Services
{
    public static class FaceService
    {
        private static readonly HttpClient client = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Constants.LocalEndpoint);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public async static Task<FaceInfo> AnalyzePhoto(MediaFile photo)
        {
            try
            {
                var content = new StreamContent(photo.GetStream());
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                var response = await client.PostAsync($"{Constants.LocalService}?{Constants.RequestParameters}", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FaceInfo[]>(json);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }

            return new FaceInfo();
        }
    }
}