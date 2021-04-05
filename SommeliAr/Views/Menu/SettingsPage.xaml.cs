using System;
using System.Collections.Generic;
using SommeliAr.Models;
using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void TasteSet_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            /*var result = new Token();
            if (result != null)
            {
                Application.Current.MainPage = new Tastes();
            }
            */

            Navigation.PushAsync(new Tastes());
        }
    }
}
