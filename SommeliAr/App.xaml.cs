﻿using System;
using SommeliAr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SommeliAr
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Tastes());
          
         
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
