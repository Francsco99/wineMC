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
    public partial class ChangeUserNamePage : ContentPage
    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public ChangeUserNamePage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            }
            InitializeComponent();
        }

       async void Change_username_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            var user = new MyUser();
            var newUsername = New_username_entry.Text;
            bool usernameOk;
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            void UsernameValidation()
            {
                //deve contenere tra i 6 e i 18 caratteri alfanumerici
                var userPattern = "[A-Za-z][A-Za-z0-9._]{6,18}";         

                if (newUsername != null)
                {
                    if (Regex.IsMatch(newUsername, userPattern))
                    {
                        usernameOk = true;
                        LabelUserError.TextColor = Color.White;
                    }

                    else
                    {
                        usernameOk = false;
                        LabelUserError.TextColor = Color.Red;
                    }
                }
                else
                {
                    usernameOk = false;
                    LabelUserError.TextColor = Color.Red;
                }
            }

            UsernameValidation();

            if (usernameOk)
            {
                try
                {
                    //This is the saved firebaseauthentication that was saved during the time of login
                    var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyLoginToken", ""));
                    //Here we are Refreshing the token
                    var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                    Preferences.Set("MyLoginToken", JsonConvert.SerializeObject(RefreshedContent));
                    //Cambio password utente
                    await authProvider.UpdateProfileAsync(savedfirebaseauth.FirebaseToken, newUsername, "");
                    await App.Current.MainPage.DisplayAlert("Success!", "New Username set correctly!", "OK");
                    await Navigation.PushAsync(new LoginPage());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Something went wrong!", "Try to logout and log back in", "OK");
                }
            }
        }
    }
}
