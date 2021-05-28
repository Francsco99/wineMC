using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.SettingsViews
{
    public partial class ContactUsPage : ContentPage
    {
        string probability;

        private string SetDisplayLabelText()
        {
            if (Preferences.Get("Probability", "") != null)
            {
                return Preferences.Get("Probability", "");
            }
            else
            {
                return "Probability value not set";
            }
        }

        public ContactUsPage()
        {
            InitializeComponent();
            SetDisplayLabelText();
        }


        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            displayLabel.Text = String.Format("Minimum probability value {0:F1}", value);
            decimal decimalValue = Math.Round((decimal)value, 1);
            probability = decimalValue.ToString();
        }

        void Set_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Set("Probability", probability);
            displayLabel.Text = "Minimum probability value "+Preferences.Get("Probability", "");
        }

        void Clear_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("Probability");
            displayLabel.Text = "Probability value not set";
        }
    }
}
