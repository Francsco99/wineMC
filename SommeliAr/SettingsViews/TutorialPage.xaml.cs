using System;
using System.Collections.Generic;
using SommeliAr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.SettingsViews
{
    public partial class TutorialPage : ContentPage
    {
        public TutorialPage()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
        }




        /*
        List<string> wineNames = new List<string>();
        

        async void Test_cronologia_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            wineNames.Add("San Crispino");
            wineNames.Add("Barolo");

            await DBFirebase.Instance.AddHistoryWines(wineNames, Preferences.Get("UserEmailFirebase", ""));
        }*/
    }
}
