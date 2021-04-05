using System;
using System.Collections.Generic;
using SommeliAr.Models;
using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class MasterDetail : TabbedPage
    {
        public MasterDetail()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Avanti_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = new Token();
            if (result != null)
            {
                
            }
        }


    }
}
