using System;
using System.Collections.Generic;
using SommeliAr.Models;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using Xamarin.Forms;

namespace SommeliAr.SettingsViews
{
    public partial class AddNewWinePage : ContentPage
    {
        public AddNewWinePage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            }
            InitializeComponent();
            BindingContext = new AllWinesViewModel();   
        }

        void All_wines_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AllWinesPage());
        }

        async void Add_wine_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("Success!", "Wine correctly added.", "Ok");
           await Navigation.PushAsync(new AllWinesPage());
        }
    }

}
