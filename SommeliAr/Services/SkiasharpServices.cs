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
    }
}
