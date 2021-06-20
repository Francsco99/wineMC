using System;
using System.IO;
using SkiaSharp;

namespace SommeliAr.Services
{
    public class SkiasharpServices
    {

        public SkiasharpServices()
        {

        }

        /*Ruota l'immagine, usato per iOS*/
        public static SKBitmap Rotate(Stream stream)
        {
            using (var bitmap = SKBitmap.Decode(stream))
            {
                var rotated = new SKBitmap(bitmap.Height, bitmap.Width);

                using (SKCanvas canvas = new SKCanvas(rotated))
                {
                    canvas.Clear();
                    canvas.Translate(bitmap.Height, 0);
                    canvas.RotateDegrees(90);
                    canvas.DrawBitmap(bitmap, new SKPoint());
                }
                return rotated;
            }
        }

        /*URL di customVision*/
        public static String GetCustomVisionURL()
        {
            return "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/25297b8e-0359-4a42-bd3e-8fcc1ed8b3f5/detect/iterations/Iteration7/image";
        }

        /*chiave API di customVision*/
        public static String GetAPIKey()
        {
            return "0b8a0ea4568b49a68d802140f9c494d1";
        }
    }
}
