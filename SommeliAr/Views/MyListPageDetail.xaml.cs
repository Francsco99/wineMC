using System;
using System.Collections.Generic;
using SommeliAr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class MyListPageDetail : ContentPage
    {
        private string itemName { get; set; }
        private string itemDescription { get; set; }
        private string itemImageSrc { get; set; }
        private string voto { get; set; }

        public MyListPageDetail(string Name, string Description, string Source, string Rating)
        {
            InitializeComponent();

            this.itemName = Name;
            this.itemDescription = Description;
            this.itemImageSrc = Source;
            this.voto = Rating;
            MyItemNameShow.Text = Name;
            MyDescriptionShow.Text = Description;
            Voto.Text =  voto;
            MyImageCall.Source = new UriImageSource()
            {
                Uri = new Uri(Source)
            };

        }

        async void Add_to_fav_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            new Command(async () => await DBFirebase.Instance.AddFavWine(itemName, Preferences.Get("UserEmailFirebase", ""))).Execute(null);
            await DisplayAlert("Success!", itemName + " added correctly", "Ok");

        }
    }
}
