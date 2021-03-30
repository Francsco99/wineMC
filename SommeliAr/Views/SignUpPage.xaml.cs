using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SommeliAr.Models;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        void SignInProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text, Entry_Email.Text);
            if (user.CheckInformation())
            {
                DisplayAlert("Login", "Login Success", "Okay");
            }
            else
            {
                DisplayAlert("Login", "Login Not correct, Empty username or password.", "Oke");
            }

            if (user.IsPasswordMatching(Entry_Password.Text, Entry_ConfirmPassword.Text))
            {

                DisplayAlert("Login", "Login Success", "Okay");
            }
            else
            {
                DisplayAlert("Login", "Login Not correct, Empty username or password.", "Oke");
            }



        }

        private void HideButton_Clicked(object sender, EventArgs e)
        {
            if (Entry_Username.IsPassword == true)
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            var email = Entry_Email.Text;

            /* uso emailregex per verificare se la mail inserita ha una formattazione valida*/
            var emailPattern =
                "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

            if (email != null)
            {
                if (Regex.IsMatch(email, emailPattern))
                    ErrorLabelText.Opacity = 0;
                else
                    ErrorLabelText.Opacity = 0.7;
            }

            else
                ErrorLabelText.Opacity = 0.7;

        }
    }

}
