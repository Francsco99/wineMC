using System;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class SettingsPage : ContentPage
    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            GetProfileInformationAndRefreshToken();

        }

        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyLoginToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyLoginToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                user_info_txt_cell.Text = savedfirebaseauth.User.Email;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }
        }

        void logout_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("MyLoginToken");
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        void change_tastes_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Tastes());
        }

        async private void change_username_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            string newUsrname = await DisplayPromptAsync("Change Username", "Enter the new username here", "OK", "Cancel", "New username");

        }

        async void change_pwd_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            string newPwd = await DisplayPromptAsync("Change Password", "Enter the new password here", "OK", "Cancel", "New password");

        }
    }
}
