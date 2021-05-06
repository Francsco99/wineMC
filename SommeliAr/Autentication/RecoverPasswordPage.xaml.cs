using System;
using Firebase.Auth;
using SommeliAr.Services;
using Xamarin.Forms;

namespace SommeliAr.Views.Autentication
{
    public partial class RecoverPasswordPage : ContentPage
    {
        public RecoverPasswordPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        async void ForgotPwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            var Email = Forgot_email.Text;

            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(AuthFirebase.Instance.GetKey()));
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
