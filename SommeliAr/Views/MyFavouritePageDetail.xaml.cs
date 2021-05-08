using System;
using System.Collections.Generic;
using SommeliAr.Menu;
using SommeliAr.Services;
using SommeliAr.Views.Menu;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class MyFavouritePageDetail : ContentPage
    {
        private string itemName { get; set; }
        private string itemDescription { get; set; }
        private string itemImageSrc { get; set; }
        private string voto { get; set; }
        public MyFavouritePageDetail(string Name, string Description, string Source, string Rating)
        {
            InitializeComponent();

            this.itemName = Name;
            this.itemDescription = Description;
            this.itemImageSrc = Source;
            this.voto = Rating;
            MyItemNameShow.Text = Name;
            MyDescriptionShow.Text = Description;
            Voto.Text = "Rating: " + voto +" /100";
            MyImageCall.Source = new UriImageSource()
            {
                Uri = new Uri(Source)
            };

        }

        async void Remove_from_fav_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            await DBFirebase.Instance.DeleteFavWine(itemName, Preferences.Get("UserEmailFirebase", ""));
            await DisplayAlert("Success!", itemName + " removed correctly", "Ok");
            Navigation.RemovePage(this);
        }
    }
}
