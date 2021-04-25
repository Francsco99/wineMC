using System;
using System.Collections.Generic;
using SommeliAr.Services;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class MyListPageDetail : ContentPage
    {
        private string itemName { get; set; }
        private string itemDescription { get; set; }
        private string itemImageSrc { get; set; }

        public MyListPageDetail(string Name, string Description, string Source)
        {
            InitializeComponent();

            this.itemName = Name;
            this.itemDescription = Description;
            this.itemImageSrc = Source;

            MyItemNameShow.Text = Name;
            MyDescriptionShow.Text = Description;
            MyImageCall.Source = new UriImageSource()
            {
                Uri = new Uri(Source)
            };

        }

        void Add_to_fav_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            new Command(async () => await new DBFirebase().AddFavouriteWine(itemName)).Execute(null);
        }
    }
}
