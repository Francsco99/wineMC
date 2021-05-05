using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Models;
using SommeliAr.Services;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Menu
{
    public partial class MyHomePage : ContentPage
    {     
        public MyHomePage()
        {
            InitializeComponent();
            //Toglie le righette di separazione delle entry della listview
            my_list_view.SeparatorVisibility = (SeparatorVisibility)1;
            GetProfileInformationAndRefreshToken();
        }

        private static Color violetto = Color.FromHex("#8b52ff");

        private void GetProfileInformationAndRefreshToken()
        {
            AuthFirebase services = new AuthFirebase();
            User user;
            try
            {
                user = services.GetUserFromDB();
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
            catch
            {
                services.RefreshToken();
                user = services.GetUserFromDB();
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
        }

        private void DeleteWelcomeLabels()
        {
            User_name_lbl.Text = "";
            Welcome_lbl.Text = "";
        }
       
        private async void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            DeleteWelcomeLabels();
            my_list_view.IsVisible = false;
            //setting del BindingContext
            MyFavoritesPageViewModel fav = new MyFavoritesPageViewModel();
            Loading.IsVisible = true;
            await fav.GetInfoAsync();
            Loading.IsVisible = false;
            my_list_view.IsVisible = true;
            BindingContext = fav;
            my_list_view.SeparatorVisibility = 0;
          
            //setting del colore in base al tema del dispositivo
            History_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);

            History_btn.FontSize = 20;
            Favourites_btn.TextColor = violetto;
            Favourites_btn.FontSize = 30;
            
        }

        async void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {           
            DeleteWelcomeLabels();
            my_list_view.IsVisible = false;
            //setting del BindingContext
            MyHistoryPageViewModel his = new MyHistoryPageViewModel();
            Loading.IsVisible = true;
            await his.GetInfoAsync();
            Loading.IsVisible = false;
            my_list_view.IsVisible = true;
            BindingContext = his;
            my_list_view.SeparatorVisibility = 0;

            //setting del colore in base al tema del dispositivo
            Favourites_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);

            Favourites_btn.FontSize = 20;
            History_btn.TextColor = violetto;
            History_btn.FontSize = 30;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description, mydetails.Image));
        }

    }
}
