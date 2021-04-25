using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Autentication;
using SommeliAr.Services;
using SommeliAr.SettingsViews;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class SettingsPage : ContentPage
    {

        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            GetProfileInformationAndRefreshToken();
        }

        private void GetProfileInformationAndRefreshToken()
        {
            AuthFirebase services = new AuthFirebase();
            User user;
            try
            {
                user = services.GetUserInfo();
                user_email_txt_cell.Text = "Email:  " + user.Email;
                user_displayName_txt_cell.Text = "Username:  " + user.DisplayName;
                if (user.IsEmailVerified)
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
            catch
            {
                services.RefreshToken();
            }
        }

        void Logout_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("MyLoginToken");
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        void Change_tastes_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Tastes());
        }

        void Change_username_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ChangeUserNamePage());

        }

        void Change_pwd_text_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ChangePwdPage());

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
