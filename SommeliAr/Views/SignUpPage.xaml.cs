using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SommeliAr.Models;
using SommeliAr.Views.Menu;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        async void RegistrationProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text, Entry_Email.Text);
            var email = Entry_Email.Text;
            bool emailOk = false;
            bool passOk = false;
            bool userOk = false;


            /* uso emailregex per verificare se la mail inserita ha una formattazione valida*/
            var emailPattern =
                "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

            if (email != null)
            {
             
                if (Regex.IsMatch(email, emailPattern))
                {
                    ErrorLabelText.Opacity = 0;
                    LabelMailError.TextColor = Color.White;
                    emailOk = true;
                }
                else
                {
                    ErrorLabelText.Opacity = 0.7;
                    LabelMailError.TextColor = Color.Red;
                    emailOk = false;
                }
            }
            else
            {
                ErrorLabelText.Opacity = 0.7;
                emailOk = false;
                LabelMailError.TextColor = Color.Red;
            }


            if (!user.IsPasswordMatching(Entry_Password.Text, Entry_ConfirmPassword.Text))
            {
                ErrorPwdLabelText.Opacity = 0.7;
                passOk = false;
            }
            else
            {
                ErrorPwdLabelText.Opacity = 0;
                passOk = true;
            }
                passOk = true;

                if (emailOk && passOk && userOk)
                {
                    await DisplayAlert("Success", "Registration Success", "Okay");

                var result = new Token();
                if (result != null)
                {
                    Application.Current.MainPage = new MasterDetail();
                }
                
            }
        }

        void hideButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Entry_Password.IsPassword == true)
            {
                Entry_Password.IsPassword = false;
                Entry_ConfirmPassword.IsPassword = false;
            }
            else
            {
                Entry_Password.IsPassword = true;
                Entry_ConfirmPassword.IsPassword = true;
            }
        }

        void Btn_SignUp_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

    }

}
