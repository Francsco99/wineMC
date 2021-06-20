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

        /*Lista dei nomi dei vini trovati*/
        private List<string> tagnames;

        /*Mappa che ha come chiave il rettangolo del tagName del vino
         e come valore il suo nome*/
        private Dictionary<SKRect, string> rectTagNameMap;

        /*Booleano che diventa false la prima volta che viene eseguita
         l'animazione*/
        private bool animate = true;

        /*Valore di default della probabilità minima*/
        private double defaultProbabilityValue = 0.5;

        public ScanPage()
        {
            InitializeComponent();

<<<<<<< HEAD
=======
        }

        async void Media_Picker(System.Object sender, System.EventArgs e)
        {
            await scan_media_lyt.TranslateTo(0, 320, 250, Easing.SinInOut);
            await scan_media_lyt.ScaleTo(0.45, 250);
            scan_media_lbl.FadeTo(0, 500);

            await scan_lyt.TranslateTo(-124, 330, 250, Easing.SinInOut);
            await scan_lyt.ScaleTo(0.55, 250);
            scan_lbl.FadeTo(0, 500);

            // svuoto skImage ad ogni Scan
            skImage = null;
            tagnames = new List<string>();
            listbackgroundRect = new Dictionary<SKRect, string>();

            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                // fattore di compressione
                //CompressionQuality = 70,

            });

            if (file == null)
            {
                return;
            }

            var stream = file.GetStream();
            streamDraw = file.GetStream();
            var streamRotated = file.GetStream();
            Scan(stream, streamRotated);

>>>>>>> origin/master
        }

        async void Scan_btn_Clicked(System.Object sender, System.EventArgs e)
        {
<<<<<<< HEAD
            /*Le animazioni vengono eseguite solo la prima volta che si preme
             su scan button*/
            if (this.animate)
            {
                animate = false;
                await scan_lyt.TranslateTo(0, 330, 200, Easing.Linear);
                await scan_lyt.ScaleTo(0.5, 250);
                System.Threading.Thread.Sleep(250);
            }

            Preferences.Remove("ResultList");

            /*Ad ogni scan svuoto skImage e ricreo lista di tagnames e mappa rect-tag*/
=======
            await scan_lyt.TranslateTo(-124, 330, 250, Easing.SinInOut);
            await scan_lyt.ScaleTo(0.6, 250);
            scan_lbl.FadeTo(0, 500);

            await scan_media_lyt.TranslateTo(0, 320, 250, Easing.SinInOut);
            await scan_media_lyt.ScaleTo(0.35, 250);
            scan_media_lbl.FadeTo(0, 500);

            Preferences.Remove("ResultList");

            // svuoto skImage ad ogni Scan
>>>>>>> origin/master
            skImage = null;
            tagnames = new List<string>();
            rectTagNameMap = new Dictionary<SKRect, string>();

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera.", ":( No camera available.", "Ok");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                /*fattore di compressione*/
                CompressionQuality = 70,
            });

            if (file == null)
            {
                return;
            }

            var stream = file.GetStream();
            streamDraw = file.GetStream();
            var streamRotated = file.GetStream();
            Scan(stream, streamRotated);

        }

        public async void  Scan(Stream stream, Stream streamRotated) { 

            /*faccio capire all'animazione che è tempo di andare in scena*/
            Loading.IsVisible = true;

            /*esegui le predictions*/
            await MakePredictionAsync(stream);

            Loading.IsVisible = false;

            /*Bisogna ruotare l'immagine se siamo su iOS*/
            if (Device.RuntimePlatform == Device.iOS)
            {
                skImage = SkiasharpServices.Rotate(streamRotated);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                skImage = SKBitmap.Decode(streamDraw);
            }

            /*Pulisco il canvas per disegnarci sopra*/
            ImageCanvas.InvalidateSurface();

            /*Se hai trovato qualcosa allora popola la lista dei tag
             filtrandola dagli "Other Products"*/
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
                    /*ora il bottone per la lista dei risultati deve diventare visibile*/
                    After_scan_btn.IsVisible = true;
                }
                else
                {
                    After_scan_btn.IsVisible = false;
                }
            }
        }

        /*Imposta la probabilità minima*/
        private Double SetProbability()
        {
            if (Preferences.Get("Probability", "") != null)
            {
                return Convert.ToDouble(Preferences.Get("Probability", ""));
            }
            /*Valode di default probabilità*/
            else return this.defaultProbabilityValue;
        }

        /*Fai le predizioni*/
        private async Task MakePredictionAsync(Stream stream)
        {
            var current = Connectivity.NetworkAccess;

            /*Se non c'è connessione notifica l'utente e interrompi il processo*/
            if (current != NetworkAccess.Internet)
            {
                await DisplayAlert("No Connection.", "In order to scan you need internet access,\n please turn on your internet connection.", "Ok");
                return;
            }

            /*Prendo lo stream e lo faccio diventare bitmap*/
            var imageBytes = GetImageAsByteData(stream);

            /*URL di CustomVision*/
            var url = SkiasharpServices.GetCustomVisionURL();

            /*API key di CustomVision*/
            var predictionKey = SkiasharpServices.GetAPIKey();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                using (var content = new ByteArrayContent(imageBytes))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    var response = await client.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var predictions = JsonConvert.DeserializeObject<Response>(responseString);

                    /*imposta la probabilità minima*/
<<<<<<< HEAD
                    var minProbability = SetProbability();

=======
                    var setProbability = SetProbability();
                    
>>>>>>> origin/master
                    /*visualizza solo predizioni con sicurezza superiore a setProbability*/
                    var result = predictions.Predictions.Where(p => p.Probability >= minProbability);

                    predictionsResult = result;

                    /*lista tagnames filtrata dagli other products*/
                    foreach (var p in result)
                    {
                        if (!p.TagName.Contains("Products"))
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

            /*rendo la lista dei tagname composta da soli elementi distinti (filtro da eventuali doppioni)*/
            tagnames = tagnames.Distinct().ToList<String>();

            string delimiter = ",";
            string listone = String.Join(delimiter, tagnames);

            Preferences.Set("ResultList", listone);
            Navigation.PushAsync(new AfterScanPage());
        }

        /*Disegna sul canvas*/
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

        /*Disegna le predizioni levando gli other products*/
        private void DrawPredictions(SKCanvas canvas, float left, float top, float scaleWidth, float scaleHeight, IEnumerable<PredictionModel> SKPrediction)
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

        /*Pulisci il canvas*/
        private void ClearCanvas(SKImageInfo info, SKCanvas canvas)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            };

            canvas.DrawRect(info.Rect, paint);
        }

        /*Disegna le etichette(i nomi) dei vini*/
        private void LabelPrediction(SKCanvas canvas, string tag, BoundingBox box, float left, float top, float width, float height, bool addBox = true)
        {
            var scaledBoxLeft = left + (width * (float)box.Left);
            var scaledBoxWidth = width * (float)box.Width;
            var scaledBoxTop = top + (height * (float)box.Top);
            var scaledBoxHeight = height * (float)box.Height;

            if (addBox)
                DrawBox(canvas, scaledBoxLeft, scaledBoxTop, scaledBoxWidth, scaledBoxHeight);

            DrawText(canvas, tag, scaledBoxLeft, scaledBoxTop, scaledBoxWidth, scaledBoxHeight);
        }

        /*Disegna il testo dei vini*/
        private void DrawText(SKCanvas canvas, string tag, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
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

            /*dimensione tag speciale nel caso non venga identificato alcun vino*/
            if (text.Equals("Nothing detected"))
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
                /*violetto caratteristico dell'app*/
                Color = new SKColor(139, 82, 255, 120)
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

<<<<<<< HEAD
            if (!rectTagNameMap.ContainsKey(backgroundRect))
=======
            if ( (!listbackgroundRect.ContainsKey(backgroundRect)) && (! tag.Equals("Nothing detected")) )
>>>>>>> origin/master
            {
                rectTagNameMap.Add(backgroundRect, tag);
            }

            backgroundRect.Inflate(10, 10);

            canvas.DrawRoundRect(backgroundRect, 5, 5, paint);

            /*per ogni parola del tag se sborda dal riquadro del vino accorcia il nome e aggiunge ...*/
            foreach (var subtext in subs)
            {
                var textWidth = textPaint.MeasureText(subtext);
                String subShortened = subtext;

                if (Array.IndexOf(subs, subtext) <= 3)
                {
                    /*se il testo sborda o il nome è composto da più di quattro parole*/
                    if (textWidth > scaledBoxWidth || ((Array.IndexOf(subs, subtext) == 3) && (subs.Length >= 5)))
                    {
                        while (textPaint.MeasureText(subShortened) > scaledBoxWidth * 0.8F)
                        {
                            subShortened = subShortened.Remove(subShortened.Length - 2);
                            textWidth = textPaint.MeasureText(subShortened);
                        }
                        /*mi voglio far dare la posizione di subs per eliminare tutte le parole successive*/
                        subShortened = subShortened.Replace(subShortened, subShortened + "...");

                        textWidth = textPaint.MeasureText(subShortened);
                    }

                    /*calcolo di quanto spostare ciascuna parola per renderla centrata*/
                    xText = startLeft + (scaledBoxWidth / 2) - (textWidth / 2);
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

        private void DrawBox(SKCanvas canvas, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
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

        private void DrawBox(SKCanvas canvas, SKPaint paint, float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = CreateBoxPath(startLeft, startTop, scaledBoxWidth, scaledBoxHeight);
            canvas.DrawPath(path, paint);
        }

        private SKPath CreateBoxPath(float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = new SKPath();
            path.MoveTo(startLeft, startTop);

            path.LineTo(startLeft + scaledBoxWidth, startTop);
            path.LineTo(startLeft + scaledBoxWidth, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop);

            return path;
        }

        /*Controlla se un punto appartiene ad un rettangolo*/
        private bool CheckLocation(SKPoint point, SKRect rect)
        {
            return rect.Contains(point);
        }

        /*Gestione del tocco su un tagName*/
        private async void ImageCanvas_Touch(object sender, SKTouchEventArgs e)
        {
            /*Punto toccato dall'utente*/
            var selectedPoint = e.Location;

<<<<<<< HEAD
            /*Se la mappa rect-tag non è vuota allora procedi*/
            if (rectTagNameMap != null)
            {
                foreach (SKRect rect in rectTagNameMap.Keys)
=======
            //Console.WriteLine(selectedPoint.ToString);

            //if (listbackgroundRect != null)
            //{

                foreach (SKRect rect in listbackgroundRect.Keys)
>>>>>>> origin/master
                {
                    /*Se il punto che viene toccato sta all'interno di un rettangolo tagName procedi*/
                    if (CheckLocation(selectedPoint, rect))
                    {
                        var wineName = rectTagNameMap[rect];

<<<<<<< HEAD
                        /*Se il tag name NON è "Nothing detected" procedi*/
                        if (!wineName.Equals("Nothing detected"))
                        {
                            var vino = await DBFirebase.Instance.GetWineFromName(wineName);
                            await Navigation.PushAsync(new MyListPageDetail(vino.Name, vino.Description, vino.SensorialNotes, vino.ProductionArea, vino.Dishes, vino.Image, vino.Rating));
                        }
=======
                        var vino = await DBFirebase.Instance.GetWineFromName(wineName);
                        
                        await Navigation.PushAsync(new MyListPageDetail(vino.Name, vino.Description, vino.SensorialNotes, vino.ProductionArea, vino.Dishes, vino.Image, vino.Rating));
                        

>>>>>>> origin/master
                    }
                }
            //}
        }
    }

}