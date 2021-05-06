using SommeliAr.Menu;
using SommeliAr.Models;
using SommeliAr.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
