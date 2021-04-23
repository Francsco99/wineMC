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
            InitializeComponent();
            BindingContext = new AllWinesViewModel();
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description, mydetails.Image));

        }
    }

}
