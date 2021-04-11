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

        void Favorites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FavoritesPage());
        }
    }
}
