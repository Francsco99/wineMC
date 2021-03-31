﻿using System;
using System.Collections.Generic;
using SommeliAr.Models;
using SommeliAr.Views.Menu;
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

        async void LoginProcedure(System.Object sender, System.EventArgs e)
        {
            //TODO implementare login

           await DisplayAlert("Success", "Login Success", "Okay");

            var result = new Token();
            if (result != null)
            {
                Application.Current.MainPage = new MasterDetail();
            }
        }

        void Btn_SignUp_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
            
        }
    }
}
