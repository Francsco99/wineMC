using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        void favourites_edit_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FavoritesPage());
        }

        void recent_scans_edit_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AfterScan());
        }
    }
}
