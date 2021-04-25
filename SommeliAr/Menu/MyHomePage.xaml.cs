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
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public MyHomePage()
        {
            InitializeComponent();

            //Toglie le righette di separazione delle entry della listview
            my_list_view.SeparatorVisibility = (SeparatorVisibility)1;

            GetProfileInformationAndRefreshToken();
        }

        private void GetProfileInformationAndRefreshToken()
        {
            AuthFirebase services = new();
            User user;
            try
            {
                user = services.GetUserInfo();
                Preferences.Set("UserEmailFirebase", user.Email.Replace(".", "-").Replace("@", "-"));
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
            catch
            {
                services.RefreshToken();
                user = services.GetUserInfo()
                Preferences.Set("UserEmailFirebase", user.Email.Replace(".", "-").Replace("@", "-"));
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
        }
        
        private void DeleteWelcomeLabels()
        {
            User_name_lbl.Text = "";
            Welcome_lbl.Text = "";
        }
        
        //colore violetto chiaro
        public Color myColor = Color.FromHex("#8b52ff");

        void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            DeleteWelcomeLabels();
            BindingContext = new MyFavoritesPageViewModel();
            my_list_view.SeparatorVisibility = 0;

            //setting del colore in base al tema del dispositivo
            History_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);

            History_btn.FontSize = 20;
            Favourites_btn.TextColor = myColor;
            Favourites_btn.FontSize = 30;
        }

        void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            DeleteWelcomeLabels();
            BindingContext = new MyHistoryPageViewModel();
            my_list_view.SeparatorVisibility = 0;

            //setting del colore in base al tema del dispositivo
            Favourites_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);

            Favourites_btn.FontSize = 20;
            History_btn.TextColor = myColor;
            History_btn.FontSize = 30;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description, mydetails.Image));
        }

    }
}
