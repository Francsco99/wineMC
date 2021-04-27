using Newtonsoft.Json;
using Plugin.Media;
using SommeliAr.Menu;
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
        public ScanPage()
        {
            InitializeComponent();
            /* come se nasconde ? afterScan.Visibility = ViewStates.Gone; */
            resultsListView.BackgroundColor = Color.Transparent;
            resultsListView.On<iOS>().SetRowAnimationsEnabled(false);
        }

        async void Scan_btn_Clicked(System.Object sender, System.EventArgs e)
        {


            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
               await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                CompressionQuality= 70,
                
            });

            if (file == null)
            {
                return;
            }

            //await DisplayAlert("File Location", file.Path, "OK");

            var stream = file.GetStream();

            resultImage.Source = ImageSource.FromStream(() =>
            {
                var photostream = file.GetStream();
                return photostream;
            });


            await MakePredictionAsync(stream); 
        }

        private async Task MakePredictionAsync(Stream stream)
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                DisplayAlert("No Connection", "In order to scan you need internet access, please turn on your internet connection", "OK");
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

                    var setProbability = 0.4;  
                    
                    var result= predictions.Predictions.Where(p => p.Probability >= setProbability); /* visualizza solo predizioni con sicurezza superiore a setProbability */
                    resultsListView.ItemsSource = result;
                }
            }
        }

        private byte[] GetImageAsByteData(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        private void After_sca_btn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AfterScanPage());
        }

        void Clear_btn_Clicked(System.Object sender, System.EventArgs e)
        {
         
        }

    }

    /* CON XAMARIN ESSENTIALS var result = await MediaPicker.CapturePhotoAsync();

    if (result != null)
    {
        var stream = await result.OpenReadAsync();

        resultImage.Source = ImageSource.FromStream(() => stream);
    } */

}
