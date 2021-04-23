using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Autentication;
using SommeliAr.SettingsViews;
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
                user_email_txt_cell.Text = "Email:  " + savedfirebaseauth.User.Email;
                user_displayName_txt_cell.Text ="Username:  " + savedfirebaseauth.User.DisplayName;

                if(savedfirebaseauth.User.IsEmailVerified)
                {
                    user_is_verified_txt_cell.Text = "Verified!";
                    user_is_verified_txt_cell.TextColor = Color.Green;
                }
                else
                {
                    user_is_verified_txt_cell.Text = "Verify your Email now!";
                    user_is_verified_txt_cell.TextColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }
        }

        void Change_tastes_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Tastes());
        }

        private void Change_username_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ChangeUserNamePage());

        }

        void Change_pwd_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ChangePwdPage());

        }
   
        void Logout_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("MyLoginToken");
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        void Tutorial_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new TutorialPage());
        }

        void Add_wine_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddNewWinePage());
        }
    }
}
