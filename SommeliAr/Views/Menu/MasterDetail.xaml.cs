using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class MasterDetail : ContentPage
    {
        public MasterDetail()
        {
            InitializeComponent();
        }

        void Avanti_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SecondPage());
        }


    }
}
