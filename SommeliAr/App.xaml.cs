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
