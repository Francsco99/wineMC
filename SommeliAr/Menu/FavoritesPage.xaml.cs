using System;
using System.Collections.Generic;
using SommeliAr.Menu;
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

        void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }

    }

}