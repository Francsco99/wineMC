using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.SettingsViews
{
    public partial class ContactUsPage : ContentPage
    {
        string probability;

        public ContactUsPage()
        {
            InitializeComponent();
            displayLabel.Text = Preferences.Get("Probability", "");
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {           
            double value = args.NewValue;
            //rotatingLabel.Rotation = value;
            //displayLabel.Text = String.Format("The Slider value is {0:F1}", value);
            decimal decimalValue = Math.Round((decimal)value, 1);
            probability = decimalValue.ToString();
        }

        void set_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Set("Probability", probability);
            displayLabel.Text = Preferences.Get("Probability", "");
        }

        void clear_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("Probability");
            displayLabel.Text = "(uninitialized)";
        }
    }
}
