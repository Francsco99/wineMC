using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Models;
using SommeliAr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Autentication
{
    public partial class ChangePwdPage : ContentPage
    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public ChangePwdPage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = Color.Transparent;
            }
            InitializeComponent();
        }


        async void change_pwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            var user = new MyUser();
            var userNewPwd = new_pwd_entry.Text;
            var userConfPwd = repeat_pwd_entry.Text;
            

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            bool passwordOk;
            void PasswordValidation()
            {
                //tra 8-20 cifre, un carattere maiuscolo, un carattere minuscolo, un numero e un carattere speciale
                var passwordPattern = "(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}";

                if (userNewPwd != null)
                {
                    if (Regex.IsMatch(userNewPwd, passwordPattern))
                    {
                        passwordOk = true;
                        LabelPwdError.TextColor = Color.Transparent;

                    }

                    else
                    {
                        passwordOk = false;
                        LabelPwdError.TextColor = Color.Red;

                    }
                }
                else
                {
                    passwordOk = false;
                    LabelPwdError.TextColor = Color.Red;

                }
            }
            void PasswordConfirmationValidation()
            {
                if (!user.IsPasswordMatching(userNewPwd, userConfPwd))
                {
                    passwordOk = false;
                    LabelConfirmPwdError.TextColor = Color.Red;
                }
                else
                {
                    passwordOk = true;
                    LabelConfirmPwdError.TextColor = Color.Transparent;
                }
            }

            PasswordValidation();
            PasswordConfirmationValidation();

            if (passwordOk)
            {
                try
                {
                    //This is the saved firebaseauthentication that was saved during the time of login
                    var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyLoginToken", ""));
                    //Here we are Refreshing the token
                    var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                    Preferences.Set("MyLoginToken", JsonConvert.SerializeObject(RefreshedContent));
                    //Cambio password utente
                    await authProvider.ChangeUserPassword(savedfirebaseauth.FirebaseToken, userNewPwd);
                    await App.Current.MainPage.DisplayAlert("Success.", "New password set correctly.", "Ok");
                    await Navigation.PushAsync(new LoginPage());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Ops... Something went wrong.", "Try to logout and log back in.", "Ok");
                }
            }
        }

    }
}
