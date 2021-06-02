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

            User user;
            try
            {
                user = AuthFirebase.Instance.GetUserFromDB();
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
                AuthFirebase.Instance.RefreshToken();
            }
        }

        private string CheckProbabilityPreference()
        {
            if (Preferences.Get("Probability", "") != null)
            {
                return "Min. Probability: " + Preferences.Get("Probability", "");
            }
            else
            {
                return "Min. Probability: not set yet.";
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

        void Contact_us_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ContactUsPage());
        }

        void Min_probability_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            min_probability_txt_cell.Text = CheckProbabilityPreference();
        }

        void Our_website_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new OurWebsitePage());
        }

        async void Delete_history_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                await DBFirebase.Instance.DeleteHistory(Preferences.Get("UserEmailFirebase", ""));
                await DisplayAlert("Success!", "History cleared.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Oops!", "Something went wrong, try again!", "Ok");
                Console.WriteLine(ex.Message);
            }
           
           
        }

       async void Delete_all_favourites_txt_cell_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                await DisplayAlert("Attention!", "You are going to delete ALL your favourite wines,\n are you sure?", "Yes","No");
                await DBFirebase.Instance.DeleteAllFavourites(Preferences.Get("UserEmailFirebase", ""));
                await DisplayAlert("Success!", "Favourites cleared.", "Ok");
            }
            catch(Exception ex)
            {
                await DisplayAlert("Oops!", "Something went wrong, try again!", "Ok");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
