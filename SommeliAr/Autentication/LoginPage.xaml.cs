using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Services;
using SommeliAr.Views.Autentication;
using SommeliAr.Views.Menu;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class LoginPage : ContentPage
    {
       
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //procedura per il login
        async void Sign_in_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Entry_Email.Text != null && Entry_Password.Text != null)
            {
                string emailLowerCase = Entry_Email.Text.ToLower();

                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(AuthFirebase.Instance.GetKey()));
                try
                {
                    FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(Entry_Email.Text, Entry_Password.Text);
                    FirebaseAuthLink content = await auth.LinkToAsync(Entry_Email.Text, Entry_Password.Text);
                    string serializedcontnet = JsonConvert.SerializeObject(content);

                    Preferences.Set("MyLoginToken", serializedcontnet);
                    Preferences.Set("UserEmailFirebase", emailLowerCase.Replace(".", "-").Replace("@", "-at-"));
                    await Navigation.PushAsync(new MasterDetail());

                    if (content.User.IsEmailVerified == false)
                    {
                        bool action = await App.Current.MainPage.DisplayAlert("Alert!", "Your account is not verified yet, Send verification email again?", "Yes", "No");
                        if (action)
                        {
                            await authProvider.SendEmailVerificationAsync(content.FirebaseToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Alert!", "Invalid Email or password", "OK");
                }
            }
        }

        //bottone password dimenticata
        void Forgot_pwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RecoverPasswordPage());
        }

        //bottone che nasconde la password
        void Hide_pwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Entry_Password.IsPassword == true)
            {
                Entry_Password.IsPassword = false;
            }
            else
            {
                Entry_Password.IsPassword = true;
            }
        }

        //bottone sign up now
        void Sign_up_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}
