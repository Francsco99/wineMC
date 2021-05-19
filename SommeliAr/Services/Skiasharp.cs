using System;
using System.IO;
using SkiaSharp;

namespace SommeliAr.Services
{
    public class Skiasharp
    {
        public Skiasharp()
        {

        }

        public static SKBitmap Rotate(Stream stream)
        {
            using (var bitmap = SKBitmap.Decode(stream))
            {
                var rotated = new SKBitmap(bitmap.Height, bitmap.Width);

                using (var surface = new SKCanvas(rotated))
                {
                    surface.Translate(rotated.Width, 0);
                    surface.RotateDegrees(90);
                    surface.DrawBitmap(bitmap, 0, 0);
                }

                return rotated;
            }
        }
    }
}
