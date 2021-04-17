using System;
using System.Collections.Generic;
using Firebase.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views.Autentication
{
    public partial class RecoverPasswordPage : ContentPage
    {
        public string WebAPIKey = "AIzaSyC2oBxLJjJPEJ_0qZE4DFWfAGdoNRTzWPE";

        public RecoverPasswordPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void forgotPwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            var Email = Forgot_email.Text;

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                await authProvider.SendPasswordResetEmailAsync(Email);
                await App.Current.MainPage.DisplayAlert("Alert", "Email Sent", "Ok");
                await Navigation.PushAsync(new LoginPage());
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid Email or password", "OK");
            }
        }
    }
}
