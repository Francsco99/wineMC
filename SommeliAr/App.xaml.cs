using System;
using SommeliAr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SommeliAr.Views.Menu;
using Xamarin.Essentials;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;

namespace SommeliAr
{
    public partial class App : Application
    {
        public App()
        {
            // Add your Azure Custom Vision endpoint
            string ENDPOINT = "ec60587f247345e7989172e6cd3c1ca5";

            // Add your prediction key from the settings page of the portal
            string predictionKey = "0b8a0ea4568b49a68d802140f9c494d1";

            {
                InitializeComponent();
                if (!string.IsNullOrEmpty(Preferences.Get("MyLoginToken", "")))
                {
                    MainPage = new NavigationPage(new MasterDetail());
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
