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
        private string sensorialNotes { get; set; }
        private string productionArea { get; set; }
        private string dishes { get; set; }
        private string voto { get; set; }

        public MyListPageDetail(string Name,
            string Description, string SensorialNotes,
            string ProductionArea, string Dishes, string Source, string Rating)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
                if (WineName != null)
                {
                    Thickness margin = WineName.Margin;
                    margin.Top = 60;
                    WineName.Margin = margin;
                }
            }


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

        private Color SetFrameColor(string voto)
        {
            double votoDouble = Convert.ToDouble(voto); // non funziona bene con i voti puntati: es 7.2

            if (votoDouble >= 0.0 && votoDouble < 6.0)
            {
                return Color.Red;
            }

            else if (votoDouble >= 6.0 && votoDouble <= 8.0)
            {
                return Color.Gold;
            }

            else 
            {
                return Color.Green;
            }
        }
        
        async void Add_to_fav_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            new Command(async () =>
            await DBFirebase.Instance.AddFavWine(itemName, Preferences.Get("UserEmailFirebase", ""))).Execute(null);

            await DisplayAlert("Success!", itemName + " added correctly", "Ok");

        }
    }
}
