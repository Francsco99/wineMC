using SommeliAr.Menu;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SommeliAr.Menu
{
    public partial class AfterScanPage : ContentPage
    {
        public AfterScanPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            
        }

        private void Favourite_Clicked(object sender, EventArgs e)
        {
            StarView.PlayAnimation();
        }
    }
}
