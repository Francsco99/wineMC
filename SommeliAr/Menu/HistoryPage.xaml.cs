using System;
using System.Collections.Generic;
using SommeliAr.Views.Menu;
using Xamarin.Forms;

namespace SommeliAr.Menu
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FavoritesPage());
        }
    }
}
