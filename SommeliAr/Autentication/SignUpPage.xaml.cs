using System;
using System.Text.RegularExpressions;
using Firebase.Auth;
using SommeliAr.Models;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class SignUpPage : ContentPage
    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        async void RegistrationProcedure(object sender, EventArgs e)
        {
            MyUser user = new MyUser(Entry_Username.Text, Entry_Password.Text, Entry_Email.Text);
            var username = Entry_Username.Text;
            var email = Entry_Email.Text;
            var pwd = Entry_Password.Text;
        

            bool emailOk = false;
            bool passOk = false;
            bool userOk = false;
            bool ageOk = false;

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
            void PasswordConfirmationValidation()
            {
                if (!user.IsPasswordMatching(Entry_Password.Text, Entry_ConfirmPassword.Text))
                {
                    ErrorPwdLabelText.Opacity = 0.7;
                    passOk = false;
                    LabelConfirmPwdError.TextColor = Color.Red;
                }
                else
                {
                    ErrorPwdLabelText.Opacity = 0;
                    passOk = true;
                    LabelConfirmPwdError.TextColor = Color.White;
                }
            }
            void UserValidation()
            {
                var userPattern = "[A-Za-z][A-Za-z0-9._]{6,18}";         /* deve contenere tra i 6 e i 18 caratteri alfanumerici */

                if (username != null)
                {
                    if (Regex.IsMatch(username, userPattern))
                    {
                        userOk = true;
                        LabelUserError.TextColor = Color.White;
                        UserErrorIcon.Opacity = 0;
                    }

                    else
                    {
                        userOk = false;
                        LabelUserError.TextColor = Color.Red;
                        UserErrorIcon.Opacity = 0.7;
                    }
                }
                else
                {
                    userOk = false;
                    LabelUserError.TextColor = Color.Red;
                    UserErrorIcon.Opacity = 0.7;
                }
            }
            void PasswordValidation()
            {
                var passwordPattern = "(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}";  /* tra 8-20 cifre, un carattere maiuscolo, un carattere minuscolo, un numero e un carattere speciale*/

                if (pwd != null)
                {
                    if (Regex.IsMatch(pwd, passwordPattern))
                    {
                        passOk = true;
                        LabelPwdError.TextColor = Color.White;
                        PwdErrorIcon.Opacity = 0;
                    }

                    else
                    {
                        passOk = false;
                        LabelPwdError.TextColor = Color.Red;
                        PwdErrorIcon.Opacity = 0.7;
                    }
                }
                else
                {
                    passOk = false;
                    LabelPwdError.TextColor = Color.Red;
                    PwdErrorIcon.Opacity = 0.7;
                }
            }
            void AgeValidation()
            {
                DateTime todayDate = DateTime.Now;
                int timespan = (todayDate - BirthDate.Date).Days;

                if (timespan >= 6570)
                {
                    ageOk = true;
                    ErrorBirthLabelText.TextColor = Color.White;
                    ErrorBirth.Opacity = 0;
                }

                else
                {
                    ageOk = false;
                    ErrorBirthLabelText.TextColor = Color.Red;
                    ErrorBirth.Opacity = 0.7;

                }
            }

            UserValidation();
            MailValidation();
            PasswordValidation();
            PasswordConfirmationValidation();
            AgeValidation();

            if (emailOk && passOk && userOk && ageOk) /* se tutti i campi sono rispettati la procedura ha successo */
            { 
                try
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Entry_Email.Text, Entry_Password.Text);
                    var usr = await authProvider.UpdateProfileAsync(auth.FirebaseToken, username,"");
                    string gettoken = auth.FirebaseToken;
                    await App.Current.MainPage.DisplayAlert("Alert","Sign Up Success!","Ok");
                    await Navigation.PushAsync(new LoginPage());
                     }
                catch(Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert",ex.Message, "Ok");
                }

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
            Navigation.PushAsync(new LoginPage());

        }
    }
}
