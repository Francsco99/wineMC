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

        void SignInProcedure (object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.checkInformation())
            {
                DisplayAlert("Login", "Login Success", "Oke");
            }
            else
            {
                DisplayAlert("Login", "Login Not correct, Empty username or password.", "Oke");
            }


        }
    }
}
