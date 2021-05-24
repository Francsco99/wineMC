using System;
using System.Collections.Generic;
using SommeliAr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.SettingsViews
{
    public partial class TutorialPage : ContentPage
    {
        public TutorialPage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            }
            InitializeComponent();
        }        
    }
}
