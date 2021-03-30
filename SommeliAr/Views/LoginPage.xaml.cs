using System;
using System.Collections.Generic;
using SommeliAr.Models;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        void hideButton_Clicked(System.Object sender, System.EventArgs e)
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

        void LoginProcedure(System.Object sender, System.EventArgs e)
        {
            //TODO implementare login

            DisplayAlert("Success", "Login Success", "Okay");
        }

        
    }
}
