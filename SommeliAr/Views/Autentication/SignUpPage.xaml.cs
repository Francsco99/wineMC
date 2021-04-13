using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SommeliAr.Models;
using SommeliAr.Views.Menu;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        void RegistrationProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text, Entry_Email.Text);
            var email = Entry_Email.Text;
            bool emailOk = false;
            bool passOk = false;
            bool userOk = false;

            void MailValidation()
            {
                var emailPattern = /* espressione regolare per verificare se la mail inserita ha una formattazione valida*/
                    "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

                if (email != null)
                {
                    if (Regex.IsMatch(email, emailPattern)) emailOk = true;
                    else emailOk = false;

                    switch (emailOk)
                    {
                        case true:
                            ErrorLabelText.Opacity = 0;
                            LabelMailError.TextColor = Color.White;
                            break;

                        case false:
                            ErrorLabelText.Opacity = 0.7;
                            LabelMailError.TextColor = Color.Red;
                            break;
                    }
                }
                else
                {
                    ErrorLabelText.Opacity = 0.7;
                    emailOk = false;
                    LabelMailError.TextColor = Color.Red;
                }
            }
            void PasswordValidation()
            {
                if (!user.IsPasswordMatching(Entry_Password.Text, Entry_ConfirmPassword.Text))
                {
                    ErrorPwdLabelText.Opacity = 0.7;
                    passOk = false;
                }
                else
                {
                    ErrorPwdLabelText.Opacity = 0;
                    passOk = true;
                }
            }

            MailValidation();
            PasswordValidation();

            if (emailOk && passOk /*&& userOk*/) /* se tutti i campi sono rispettati la procedura ha successo */
            {
                DisplayAlert("Success", "Registration Success", "Okay");

                Navigation.PushAsync(new MasterDetail());


            }
        }

        void hideButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Entry_Password.IsPassword == true)
            {
                Entry_Password.IsPassword = false;
                Entry_ConfirmPassword.IsPassword = false;
            }
            else
            {
                Entry_Password.IsPassword = true;
                Entry_ConfirmPassword.IsPassword = true;
            }
        }

        void Btn_SignUp_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

    }

}
