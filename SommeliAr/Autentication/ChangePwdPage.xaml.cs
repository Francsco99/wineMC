using System;
using System.Collections.Generic;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Autentication
{
    public partial class ChangePwdPage : ContentPage
    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public ChangePwdPage()
        {
            InitializeComponent();

        }



        async void change_pwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            var userEmail = email_entry.Text;
            var userOldPwd = old_pwd_entry.Text;
            var userNewPwd = new_pwd_entry.Text;

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(userEmail, userOldPwd);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                await authProvider.ChangeUserPassword(serializedcontnet, userNewPwd);
                await App.Current.MainPage.DisplayAlert("Alert", "Password changed", "ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

    }
}