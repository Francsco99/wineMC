using System;
using System.Collections.Generic;
using SommeliAr.Models;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using Xamarin.Forms;

namespace SommeliAr.SettingsViews
{
    public partial class AllWinesPage : ContentPage
    {
        public AllWinesPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
                Thickness margin = my_list_view.Margin;
                margin.Top = 35;
                my_list_view.Margin = margin;
            }
            BindingContext = new AllWinesViewModel();
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description, mydetails.SensorialNotes, mydetails.ProductionArea, mydetails.Dishes, mydetails.Image, mydetails.Rating));

        }
    }
}
