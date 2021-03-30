﻿using System;
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

        void RegistrationProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text, Entry_Email.Text);
            var email = Entry_Email.Text;
            bool emailOk = false;
            bool passOk = false;

            /* uso emailregex per verificare se la mail inserita ha una formattazione valida*/
            var emailPattern =
                "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

            if (email != null)
            {
             
                if (Regex.IsMatch(email, emailPattern))
                {
                    ErrorLabelText.Opacity = 0;
                    emailOk = true;
                }
                else
                {
                    ErrorLabelText.Opacity = 0.7;
                    emailOk = false;
                }
            }
            else
            {
                ErrorLabelText.Opacity = 0.7;
                emailOk = false;
            }


            if (!user.IsPasswordMatching(Entry_Password.Text, Entry_ConfirmPassword.Text))
            {
                DisplayAlert("Error", "Passwords not matching", "Okay");
                passOk = false;
            }
            else
                passOk = true;

            if (emailOk && passOk)
            {
                DisplayAlert("Success", "Registration Success", "Okay");

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

    }

}
