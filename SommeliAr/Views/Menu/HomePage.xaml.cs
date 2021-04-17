using System;
using System.Collections.Generic;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class HomePage : ContentPage
    {
        public string WebAPIKey = "AIzaSyAwqkWptVDG5gJ9VHue7AffKx5b1KqloJg";

        public HomePage()
        {
            InitializeComponent();
        }

        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                Wb_lbl.Text = savedfirebaseauth.User.Email;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }

        }

        void favourites_edit_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FavoritesPage());
        }

        void recent_scans_edit_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AfterScan());
        }
    }
}
