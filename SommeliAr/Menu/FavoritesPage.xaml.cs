using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class FavoritesPage : ContentPage
    {
        public FavoritesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public bool isFavorite = false;

        void star_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("favourite button clicked"); //scrive un messaggio sulla console


            /* var selectedList = (Result)seletecedButtonItem;

             if (selectedList.isFavorite == "Fav.png")
             {
                 // update api or local DB whatever on here
                 selectedList.isUserFavorite = false;
                 selectedList.isFavorite = "unFav.png"; // this will update in UI - changes from favorite image into unfavorite image
             }
             else
             {
                 // update api or local DB whatever on here
                 selectedList.isUserFavorite = true;
                 selectedList.favourite = "Fav.png"; // this will update in UI
             }
         }*/

        }
    }

}