using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class FavoritesPage : ContentPage
    {
        public FavoritesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void Favourite_Clicked(object sender, EventArgs e)
        {
            etichetta.Text = "ciaone";
        }
    }
}
