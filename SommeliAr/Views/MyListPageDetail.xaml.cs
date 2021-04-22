using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class MyListPageDetail : ContentPage
    {
        public MyListPageDetail(string Name, string Ingredients, string Source)
        {
            InitializeComponent();

            MyItemNameShow.Text = Name;
            MyIngredientItemShow.Text = Ingredients;
            MyImageCall.Source = new UriImageSource()
            {
                Uri = new Uri(Source)
            };
        }
    }
}
