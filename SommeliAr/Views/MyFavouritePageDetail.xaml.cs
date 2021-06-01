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
        private string sensorialNotes { get; set; }
        private string productionArea { get; set; }
        private string dishes { get; set; }
        private string itemImageSrc { get; set; }
        private string voto { get; set; }

        private void AndroidFavouriteInitalization()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
                if (WineNameFavourite != null)
                {
                    Thickness margin = WineNameFavourite.Margin;
                    margin.Top = 60;
                    WineNameFavourite.Margin = margin;
                }
            }
        }

        private Color SetFrameColor(string voto)
        {
            double votoDouble = Convert.ToDouble(voto);

            if (votoDouble >= 0 && votoDouble < 6)
            {
                return Color.Red;
            }

            else if (votoDouble >= 6 && votoDouble <= 8)
            {
                return Color.Gold;
            }

            else
                return Color.Green;
        }

        public MyFavouritePageDetail(string Name, string Description,
            string SensorialNotes, string ProductionArea,
            string Dishes, string Source, string Rating)
        {
            InitializeComponent();
            AndroidFavouriteInitalization();
            this.itemName = Name;
            this.itemDescription = Description;
            this.sensorialNotes = SensorialNotes;
            this.productionArea = ProductionArea;
            this.dishes = Dishes;
            this.itemImageSrc = Source;
            this.voto = Rating;
            MyItemNameShow.Text = Name;
            ProdArea.Text = ProductionArea;
            SensNotes.Text =SensorialNotes;
            MyDishes.Text = Dishes;
            Voto.Text =  voto;
            MyImageCall.Source = new UriImageSource()
            {
                Uri = new Uri(Source)
            };

            Round_frame.BackgroundColor = this.SetFrameColor(voto);
        }

        async void Remove_from_fav_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            await DBFirebase.Instance.DeleteFavWine(itemName, Preferences.Get("UserEmailFirebase", ""));
            await DisplayAlert("Success.", itemName + " removed correctly.", "Ok");
            Navigation.RemovePage(this);
        }
    }
}
