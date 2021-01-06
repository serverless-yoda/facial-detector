using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FacialApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacialController : ControllerBase
    {
        public async Task<List<byte[]>> GetFaces()
        {

            using var ms = new MemoryStream(2048);
            await Request.Body.CopyToAsync(ms);
            return ReadFaces(ms.ToArray());
        }

        private List<byte[]> ReadFaces(byte[] ms)
        {
            //read image from the source
            Mat source = Cv2.ImDecode(ms, ImreadModes.Color);

            //OPTIONAL Save for testing 
            source.SaveImage("image.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));

            //get cascadefile
            string path = Path.Combine(Directory.GetCurrentDirectory(), "CascadeFile", "haarcascade_frontalface_default.xml");

            //create classifier for object detection
            var faceCascadeClassifier = new CascadeClassifier(path);
            //faceCascadeClassifier.Load(path)

            //get all faces
            var facesDetected = faceCascadeClassifier.DetectMultiScale(source, 1.1, 7, HaarDetectionType.DoRoughSearch, new Size(70, 70));

            //create list of bytes to store faces
            var listOfFace = new List<byte[]>();

            //counter looping
            int ctr = 0;
            foreach (var face in facesDetected)
            {
                //create new face image
                var faceImage = new Mat(source, face);

                //store in the list
                listOfFace.Add(faceImage.ToBytes(".jpg"));

                //save for testing
                faceImage.SaveImage($"face{ctr}.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));

                ctr++;
            }

            return listOfFace;

        }
    }
}
