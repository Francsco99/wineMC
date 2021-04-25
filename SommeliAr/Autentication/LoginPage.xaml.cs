using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Views.Autentication;
using SommeliAr.Views.Menu;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class LoginPage : ContentPage
    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //procedura per il login
        async void Sign_in_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            FirebaseAuthProvider authProvider = new(new FirebaseConfig(WebAPIKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Entry_Email.Text, Entry_Password.Text);     
                var content = await auth.LinkToAsync(Entry_Email.Text, Entry_Password.Text);
                var serializedcontnet = JsonConvert.SerializeObject(content);
                //setting del token
                Preferences.Set("MyLoginToken", serializedcontnet);
                //setting della mail togliendo @ e .
                Preferences.Set("UserEmailFirebase", Entry_Email.Text.Replace(".","-").Replace("@","-"));
                await Navigation.PushAsync(new MasterDetail());

                if (content.User.IsEmailVerified == false)
                {
                    var action = await App.Current.MainPage.DisplayAlert("Alert!", "Your account is not verified yet, Send verification email again?", "Yes", "No");

                    if (action)
                    {
                        await authProvider.SendEmailVerificationAsync(content.FirebaseToken);
                    }

                }

            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Alert!", "Invalid Email or password", "OK");
            }
        }

        //bottone password dimenticata
        void Forgot_pwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RecoverPasswordPage());
        }

        //bottone sign up now
        void Sign_up_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}
