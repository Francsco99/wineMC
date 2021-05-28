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

        List<string> tagnames;
        List<SKRect> listbackgroundRect;
        public ScanPage()
        {
            InitializeComponent();
        }

        async void Scan_btn_Clicked(System.Object sender, System.EventArgs e)
        {

            Preferences.Remove("ResultList");

            skImage = null;                                                // svuoto skImage ad ogni Scan
            tagnames = new List<string>();

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
            var streamRotated = file.GetStream();
            Loading.IsVisible = true;                                        // faccio capire all'animazione che è tempo di andare in scena

            await MakePredictionAsync(stream);                               // esegui le predictions                    

            Loading.IsVisible = false;

            if (Device.RuntimePlatform == Device.iOS)
            {
                skImage = SkiasharpServices.Rotate(streamRotated);
            }
            else if(Device.RuntimePlatform == Device.Android)
            {
                skImage = SKBitmap.Decode(streamDraw);
            }
            ImageCanvas.InvalidateSurface();

            if (predictionsResult != null)
            {
                var predictionsFiltered = new List<string>();
                foreach (var pred in predictionsResult)
                {
                    if (!pred.TagName.Contains("Products"))
                    {
                        predictionsFiltered.Add(pred.TagName);
                    }
                }
                if (predictionsFiltered.FirstOrDefault() != null)
                {
                    After_scan_btn.IsVisible = true;                                      // ora il bottone per la lista dei risultati deve diventare visibile
                }
                else
                {
                    After_scan_btn.IsVisible = false;
                }

            }
            
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
            var url = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/25297b8e-0359-4a42-bd3e-8fcc1ed8b3f5/detect/iterations/Iteration7/image";
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

                    // probabilità minima impostata
                   var setProbability = Convert.ToDouble(Preferences.Get("Probability", ""));
                   //var setProbability = 0.7;
                    Console.WriteLine(setProbability);
                    // visualizza solo predizioni con sicurezza superiore a setProbability
                    var result = predictions.Predictions.Where(p => p.Probability >= setProbability); 

                    predictionsResult = result;

                    //lista tagnames
                    foreach (var p in result)
                    {
                        if (!p.TagName.Contains("Products") )
                        {
                            await DBFirebase.Instance.AddHistoryWines(p.TagName, Preferences.Get("UserEmailFirebase", ""));
                            tagnames.Add(p.TagName);
                        }
                    }
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
            Preferences.Remove("ResultList");
            string delimiter = ",";
            tagnames = tagnames.Distinct().ToList<String>(); // rendo la lista dei tagname composta da soli elementi distinti (filtro da eventuali doppioni)
              
            string listone = String.Join(delimiter, tagnames);

            Preferences.Set("ResultList", listone);
            Navigation.PushAsync(new AfterScanPage());
            // Console.WriteLine("resultList    "+Preferences.Get("ResultList", ""));
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
            List<PredictionModel> filteredSKPrediction = SKPrediction.ToList();
           
            foreach (var element in SKPrediction)
            {
                if (element.TagName.Contains("Other"))
                {
                    filteredSKPrediction.Remove(element);
                }
            }

            if (SKPrediction == null) return;

            if (!filteredSKPrediction.Any())
            {
                LabelPrediction(canvas, "Nothing detected", new BoundingBox(0, 0, 1, 1), left, top, scaleWidth, scaleHeight, false);
            }
            else if (filteredSKPrediction.All(p => p.BoundingBox != null))
            {
                foreach (var prediction in filteredSKPrediction)
                {
                    LabelPrediction(canvas, prediction.TagName, prediction.BoundingBox, left, top, scaleWidth, scaleHeight);
                }
            }
            else
            {
                var best = filteredSKPrediction.OrderByDescending(p => p.Probability).First();
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

            //textPaint.TextSize = 0.9f * scaledBoxWidth * textPaint.TextSize / textWidth;
            textPaint.TextSize = 40;
            
            if(text.Equals("Nothing detected"))   // dimensione tag speciale nel caso non venga identificato alcun vino
            {
                var textWidth = textPaint.MeasureText(text);
                textPaint.TextSize = 0.9f * scaledBoxWidth * textPaint.TextSize / textWidth;
            }
            //float lineHeight = textPaint.TextSize * 1.2f;
            string[] subs = text.Split(' ');

            float yText = (startTop) + (scaledBoxHeight / 6);
            float xText = 0;

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(139, 82, 255, 120)  //violetto caratteristico dell'app
            };

            xText = startLeft;
            float yBackgroundText = yText;

            if (subs.Length < 5 && (!text.Equals("Nothing detected")))
            {
                yBackgroundText += 50 * (subs.Length);
            }

            else if (!text.Equals("Nothing detected")) yBackgroundText += 250;

            else yBackgroundText += 330;

            var backgroundRect = new SKRect((startLeft + 20), yText, (startLeft + scaledBoxWidth - 20), (yBackgroundText + 20));

            //listbackgroundRect.Add(backgroundRect);   da implementare
            backgroundRect.Inflate(10, 10);
            
            canvas.DrawRoundRect(backgroundRect, 5, 5, paint);

            foreach (var subtext in subs)  // per ogni parola del tag se sborda dal riquadro del vino accorcia il nome e aggiunge ...
            {
                var textWidth = textPaint.MeasureText(subtext);
                String subShortened = subtext;

                if (Array.IndexOf(subs, subtext) <= 3) {
                    if (textWidth > scaledBoxWidth || ((Array.IndexOf(subs, subtext) == 3) && (subs.Length >= 5)))  // se il testo sborda o il nome è composto da più di quattro parole
                    {
                        while (textPaint.MeasureText(subShortened) > scaledBoxWidth * 0.8F)
                        {
                            subShortened = subShortened.Remove(subShortened.Length - 2);
                            textWidth = textPaint.MeasureText(subShortened);
                        }
                        subShortened = subShortened.Replace(subShortened, subShortened + "...");  // mi voglio far dare la posizione di subs per eliminare tutte le parole successive

                        textWidth = textPaint.MeasureText(subShortened);
                    }

                    xText = startLeft + (scaledBoxWidth / 2) - (textWidth / 2); // calcolo di quanto spostare ciascuna parola per renderla centrata
                    if (!text.Equals("Nothing detected"))
                    {
                        yText += 50;   // scalo riga per ogni parola del tag
                    }

                    else
                        yText += 150;

                    canvas.DrawText(subShortened,
                                xText,
                               yText,
                                textPaint);
                }
            }
        }

        static void DrawBox(SKCanvas canvas, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var outerstrokePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(139, 82, 255, 100),
                StrokeWidth = 7,
                PathEffect = SKPathEffect.CreateDash(new[] { 20f, 20f }, 20f)
            };
            DrawBox(canvas, outerstrokePaint, startLeft, startTop, scaledBoxWidth, scaledBoxHeight);

            var blurStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
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

        private void ImageCanvas_Touch(object sender, SKTouchEventArgs e)
        {
            var selectedPoint = e.Location;
            
            /*foreach(SKRect rect in listbackgroundRect)
            {

            }*/
        }
    }
}