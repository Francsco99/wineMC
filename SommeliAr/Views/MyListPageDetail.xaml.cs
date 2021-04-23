using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class MyListPageDetail : ContentPage
    {
        public MyListPageDetail(string Name, string Description, string Source)
        {
            InitializeComponent();

            MyItemNameShow.Text = Name;
            MyDescriptionShow.Text = Description;
            MyImageCall.Source = new UriImageSource()
            {
                Uri = new Uri(Source)
            };

        }

        void Add_to_fav_btn_Clicked(System.Object sender, System.EventArgs e)
        {

        }
    }
}
