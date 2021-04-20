using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Menu
{
    public partial class AfterScanPage : ContentPage
    {
        public AfterScanPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void Favourite_Clicked(object sender, EventArgs e)
        {
            StarView.PlayAnimation();
        }
    }
}
