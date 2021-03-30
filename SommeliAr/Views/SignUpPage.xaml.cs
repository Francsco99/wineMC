using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SommeliAr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var email = EntryUserEmail.Text;

            /* uso emailregex per verificare se la mail inserita ha una formattazione valida*/
            var emailPattern =
                "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

            if (email != null)
            {
                if (Regex.IsMatch(email, emailPattern))
                    ErrorLabelText.Opacity = 0;
                else
                    ErrorLabelText.Opacity = 0.7;
            }

            else
                ErrorLabelText.Opacity = 0.7;

        }

        private void HideButton_Clicked(object sender, EventArgs e)
        {

            if (EntryUserPassword.IsPassword == true)
            {
                EntryUserPassword.IsPassword = false;
                EntryUserConfirmPassword.IsPassword = false;
            }
            else
            {
                EntryUserPassword.IsPassword = true;
                EntryUserConfirmPassword.IsPassword = true;
            }

        }
    }

}