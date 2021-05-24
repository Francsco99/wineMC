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
            GetUserInformationAndRefreshToken();
            ResetView();
           
        }
        private bool favClicked = false;
        private bool histClicked = false;
        private static Color violetto = Color.FromHex("#8b52ff");

        private Color WineColor(string detail)
        {
            if (detail.Equals("Red"))
            {
                return Color.Red;
            }
            else return Color.Black;
        }

        private void ResetView()
        {
            History_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            Favourites_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            History_btn.FontSize = 25;
            Favourites_btn.FontSize = 25;
            ShowWelcomeLabels();

        }

        private void GetUserInformationAndRefreshToken()
        {

            User user = AuthFirebase.Instance.GetUserFromDB();
            try
            {
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
            catch
            {
                AuthFirebase.Instance.RefreshToken();
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
        }

        private void ShowWelcomeLabels()
        {
            Welcome_msg.IsVisible = true;

        }

        private void DeleteWelcomeLabels()
        {
            Welcome_msg.IsVisible = false;
        }

        async void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            this.favClicked = true;
            this.histClicked = false;
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
            History_btn.FontSize = 25;
            Favourites_btn.TextColor = violetto;
            Favourites_btn.FontSize = 28;

        }

        async void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            this.favClicked = false;
            this.histClicked = true;
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

            Favourites_btn.FontSize = 25;
            History_btn.TextColor = violetto;
            History_btn.FontSize = 28;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            if (histClicked)
            {
                await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description,mydetails.SensorialNotes,mydetails.ProductionArea,mydetails.Dishes, mydetails.Image, mydetails.Rating));
            }
            else if (favClicked)
            {
                await Navigation.PushAsync(new MyFavouritePageDetail(mydetails.Name, mydetails.Description,mydetails.SensorialNotes,mydetails.ProductionArea,mydetails.Dishes, mydetails.Image, mydetails.Rating));
            }
        }
    }
}
