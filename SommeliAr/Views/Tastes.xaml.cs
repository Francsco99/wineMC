using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace SommeliAr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tastes : ContentPage
    {
        public Tastes()
        {
            InitializeComponent();

            Preferences.Get("rosso", false);

            NavigationPage.SetHasNavigationBar(this, false);

        }

       
        private bool rosso = false;
        private bool bianco = false;
        private bool secco = false;
        private bool frizzante = false;

         async private void VinoRosso_Clicked(object sender, EventArgs e)
        {
            rosso = !rosso;
            VinoRosso.Scale = 1.2;
      
            await Task.Delay(150);
            VinoRosso.Scale = 1;
            if (rosso == true)
            {
                VinoRosso.BorderColor = Color.Black;
                Preferences.Set("rosso", true);   /* il sistema ricorda che ti piace il vino rosso*/
                RossoOK.Opacity = 0.2;
            }

            else
            {
                VinoRosso.BorderColor = Color.Transparent;
                Preferences.Remove("rosso");     /* il sistema rimuove la preferenza per il vino rosso*/
                RossoOK.Opacity = 0;
            }
         }

        async private void VinoBianco_Clicked(object sender, EventArgs e)
        {

            bianco = !bianco;
            VinoBianco.Scale = 1.3;
            await Task.Delay(150);
            VinoBianco.Scale = 1;
            if (bianco == true)
            {
                VinoBianco.BorderColor = Color.Black;
                Preferences.Set("PreferenceWhite", true);    /* il sistema ricorda che ti piace il vino bianco*/
            }

            else
            {
                VinoBianco.BorderColor = Color.Transparent;
                Preferences.Remove("PreferenceWhite");     /* il sistema rimuove la preferenza per il vino bianco*/
            }
        }

        async private void VinoSecco_Clicked(object sender, EventArgs e)
        {

            secco = !secco;
            VinoSecco.Scale = 1.3;
            await Task.Delay(150);
            VinoSecco.Scale = 1;
            if (secco == true)
            {
                VinoSecco.BorderColor = Color.Black;
                /* il sistema ricorda che ti piace il vino rosso*/
            }

            else
            {
                VinoSecco.BorderColor = Color.Transparent;
                /* il sistema rimuove la preferenza per il vino rosso*/
            }
        }

        async private void VinoFrizzante_Clicked(object sender, EventArgs e)
        {

            frizzante = !frizzante;
            VinoFrizzante.Scale = 1.3;
            await Task.Delay(150);
            VinoFrizzante.Scale = 1;
            if (frizzante == true)
            {
                VinoFrizzante.BorderColor = Color.Black;
                /* il sistema ricorda che ti piace il vino rosso*/
            }

            else
            {
                VinoFrizzante.BorderColor = Color.Transparent;
                /* il sistema rimuove la preferenza per il vino rosso*/
            }
        }
    }

}