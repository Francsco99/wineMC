using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SommeliAr.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SommeliAr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AfterScan : ContentPage
    {
        public AfterScan()
        {
            InitializeComponent();
        }

        private void Favourite_Clicked(object sender, EventArgs e)
        {
            StarView.PlayAnimation();
        }
    }
}