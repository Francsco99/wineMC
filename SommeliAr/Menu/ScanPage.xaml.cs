using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Newtonsoft.Json;
using Plugin.Media;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SommeliAr.Menu;
using SommeliAr.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SommeliAr.Views.Menu
{
    public partial class ScanPage : ContentPage
    {
        const float radius = 2.0f;
        const float xDrop = 2.0f;
        const float yDrop = 2.0f;

        IEnumerable<PredictionModel> predictionsResult;
        Stream streamDraw;
        SKBitmap skImage;

        public ScanPage()
        {
            InitializeComponent();

            resultsListView.BackgroundColor = Color.Transparent;
            resultsListView.On<iOS>().SetRowAnimationsEnabled(false);
        }

        async void Scan_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            skImage = null;                                                // svuoto skImage ad ogni Scan

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                CompressionQuality = 70,                                    // fattore di compressione
            });

            if (file == null)
            {
                return;
            }

            var stream = file.GetStream();
            streamDraw = file.GetStream();

            Loading.IsVisible = true;                                        // faccio capire all'animazione che è tempo di andare in scena

            await MakePredictionAsync(stream);                               // esegui le predictions                    

            Loading.IsVisible = false;

            skImage = SKBitmap.Decode(streamDraw);
            ImageCanvas.InvalidateSurface();

            After_scan_btn.IsVisible = true;                                      // ora il bottone per la lista dei risultati deve diventare visibile
        }

        private async Task MakePredictionAsync(Stream stream)
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                await DisplayAlert("No Connection", "In order to scan you need internet access, please turn on your internet connection", "OK");
                return;
            }
            var imageBytes = GetImageAsByteData(stream);
            var url = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/25297b8e-0359-4a42-bd3e-8fcc1ed8b3f5/detect/iterations/Iteration5/image";
            var predictionKey = "0b8a0ea4568b49a68d802140f9c494d1";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                using (var content = new ByteArrayContent(imageBytes))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    var response = await client.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var predictions = JsonConvert.DeserializeObject<Response>(responseString);

                    var setProbability = 0.6;                   // probabilità minima impostata
                    var result = predictions.Predictions.Where(p => p.Probability >= setProbability); // visualizza solo predizioni con sicurezza superiore a setProbability 

                    resultsListView.ItemsSource = result;
                    predictionsResult = result;

                    //lista tagnames
                    List<string> tagnames = new List<string>();
                    foreach (var p in predictions.Predictions)
                    {
                        tagnames.Add(p.TagName);
                    }
                    await DBFirebase.Instance.AddHistoryWines(tagnames, Preferences.Get("UserEmailFirebase", ""));
                }
            }
        }

        private byte[] GetImageAsByteData(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        private void After_scan_btn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AfterScanPage());
        }

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var canvas = args.Surface.Canvas;

            ClearCanvas(info, canvas);


            if (streamDraw != null)
            {
                if (skImage != null)
                {
                    var scale = Math.Min((float)info.Width / (float)skImage.Width, (float)info.Height / (float)skImage.Height);

                    var scaleHeight = (scale * skImage.Height);
                    var scaleWidth = (scale * skImage.Width);

                    var top = (info.Height - scaleHeight) / 2;
                    var left = (info.Width - scaleWidth) / 2;

                    canvas.DrawBitmap(skImage, new SKRect(left, top, left + scaleWidth, top + scaleHeight));
                    DrawPredictions(canvas, left, top, scaleWidth, scaleHeight, predictionsResult);
                }
            }

        }

        static void DrawPredictions(SKCanvas canvas, float left, float top, float scaleWidth, float scaleHeight, IEnumerable<PredictionModel> SKPrediction)
        {
            if (SKPrediction == null) return;

            if (!SKPrediction.Any())
            {
                LabelPrediction(canvas, "Nothing detected", new BoundingBox(0, 0, 1, 1), left, top, scaleWidth, scaleHeight, false);
            }
            else if (SKPrediction.All(p => p.BoundingBox != null))
            {
                foreach (var prediction in SKPrediction)
                {
                    LabelPrediction(canvas, prediction.TagName, prediction.BoundingBox, left, top, scaleWidth, scaleHeight);
                }
            }
            else
            {
                var best = SKPrediction.OrderByDescending(p => p.Probability).First();
                LabelPrediction(canvas, best.TagName, new BoundingBox(0, 0, 1, 1), left, top, scaleWidth, scaleHeight, false);
            }
        }

        static void ClearCanvas(SKImageInfo info, SKCanvas canvas)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            };

            canvas.DrawRect(info.Rect, paint);
        }

        static void LabelPrediction(SKCanvas canvas, string tag, BoundingBox box, float left, float top, float width, float height, bool addBox = true)
        {
            var scaledBoxLeft = left + (width * (float)box.Left);
            var scaledBoxWidth = width * (float)box.Width;
            var scaledBoxTop = top + (height * (float)box.Top);
            var scaledBoxHeight = height * (float)box.Height;

            if (addBox)
                DrawBox(canvas, scaledBoxLeft, scaledBoxTop, scaledBoxWidth, scaledBoxHeight);

            DrawText(canvas, tag, scaledBoxLeft, scaledBoxTop, scaledBoxWidth, scaledBoxHeight);
        }

        static void DrawText(SKCanvas canvas, string tag, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.White,
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };
            var text = tag;

            var textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = 0.9f * scaledBoxWidth * textPaint.TextSize / textWidth;

            var textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            var xText = (startLeft + (scaledBoxWidth / 2)) - textBounds.MidX;
            var yText = (startTop + (scaledBoxHeight / 2)) + textBounds.MidY;

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(139, 82, 255, 120)  //viola caratteristico dell'app
            };

            var backgroundRect = textBounds;
            backgroundRect.Offset(xText, yText);
            backgroundRect.Inflate(10, 10);

            canvas.DrawRoundRect(backgroundRect, 5, 5, paint);

            canvas.DrawText(text,
                            xText,
                            yText,
                            textPaint);
        }

        static void DrawBox(SKCanvas canvas, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var outerstrokePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(139, 82, 255, 100),
                StrokeWidth = 5,
                PathEffect = SKPathEffect.CreateDash(new[] { 20f, 20f }, 20f)
            };
            DrawBox(canvas, outerstrokePaint, startLeft, startTop, scaledBoxWidth, scaledBoxHeight);

            var blurStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 3,
                Color = SKColors.White,
                PathEffect = SKPathEffect.CreateDash(new[] { 20f, 20f }, 20f),
                IsAntialias = true,
                MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 0.57735f * radius + 0.5f)
            };
            DrawBox(canvas, blurStrokePaint, startLeft, startTop, scaledBoxWidth, scaledBoxHeight);
        }

        static void DrawBox(SKCanvas canvas, SKPaint paint, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = CreateBoxPath(startLeft, startTop, scaledBoxWidth, scaledBoxHeight);
            canvas.DrawPath(path, paint);
        }

        static SKPath CreateBoxPath(float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = new SKPath();
            path.MoveTo(startLeft, startTop);

            path.LineTo(startLeft + scaledBoxWidth, startTop);
            path.LineTo(startLeft + scaledBoxWidth, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop);

            return path;
        }

        void Clear_btn_Clicked(System.Object sender, System.EventArgs e)
        {

        }
    }

}
