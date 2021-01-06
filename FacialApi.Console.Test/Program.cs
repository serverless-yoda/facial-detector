using FacialDetector.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacialApi.Console.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = "oscars-2017.jpg";
            var address = "http://localhost:5000/api/Facial";
            var bytes = ImageUtility.ImageToBytes(imagePath);
            
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            using var client = new HttpClient();
            using var response = await client.PostAsync(address, byteContent);
            string apiresponse = await response.Content.ReadAsStringAsync();
            List<byte[]> faceList = JsonConvert.DeserializeObject<List<byte[]>> (apiresponse);

            if (faceList.Count > 0)
            {
                for(int i =0; i < faceList.Count; i++)
                {
                    ImageUtility.ByteToImage(faceList[i], $"image{i}.jpg");
                }
            }
        }
    }
}
