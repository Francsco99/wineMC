using System;
using System.Text.RegularExpressions;
using Firebase.Auth;
using SommeliAr.Models;
using SommeliAr.ViewModels;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class SignUpPage : ContentPage
    {
        //chiave api di firebase
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MyUsersViewModel();
        }

        async void RegistrationProcedure(object sender, EventArgs e)
        {
            MyUser user = new MyUser();
            var username = Entry_Username.Text;
            var email = Entry_Email.Text;
            var pwd = Entry_Password.Text;

            bool emailOk;
            bool passwordOk;
            bool usernameOk;
            bool ageOk;

            void MailValidation()
            {
                //espressione regolare per verificare se la mail inserita ha una formattazione valida
                string emailPattern = "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

                if (email != null)
                {
                    if (Regex.IsMatch(email, emailPattern)) emailOk = true;
                    else
                    {
                        emailOk = false;
                    }

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
                    passwordOk = false;
                    LabelConfirmPwdError.TextColor = Color.Red;
                }
                else
                {
                    ErrorPwdLabelText.Opacity = 0;
                    passwordOk = true;
                    LabelConfirmPwdError.TextColor = Color.White;
                }
            }
            void UsernameValidation()
            {
                //Deve contenere tra i 6 e i 18 caratteri alfanumerici
                string userPattern = "[A-Za-z][A-Za-z0-9._]{6,18}";

                if (username != null)
                {
                    if (Regex.IsMatch(username, userPattern))
                    {
                        usernameOk = true;
                        LabelUserError.TextColor = Color.White;
                        UserErrorIcon.Opacity = 0;
                    }

                    else
                    {
                        usernameOk = false;
                        LabelUserError.TextColor = Color.Red;
                        UserErrorIcon.Opacity = 0.7;
                    }
                }
                else
                {
                    usernameOk = false;
                    LabelUserError.TextColor = Color.Red;
                    UserErrorIcon.Opacity = 0.7;
                }
            }
            void PasswordValidation()
            {
                //tra 8-20 cifre, un carattere maiuscolo, un carattere minuscolo, un numero e un carattere speciale
                string passwordPattern = "(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}";

                if (pwd != null)
                {
                    if (Regex.IsMatch(pwd, passwordPattern))
                    {
                        passwordOk = true;
                        LabelPwdError.TextColor = Color.White;
                        PwdErrorIcon.Opacity = 0;
                    }

                    else
                    {
                        passwordOk = false;
                        LabelPwdError.TextColor = Color.Red;
                        PwdErrorIcon.Opacity = 0.7;
                    }
                }
                else
                {
                    passwordOk = false;
                    LabelPwdError.TextColor = Color.Red;
                    PwdErrorIcon.Opacity = 0.7;
                }
            }
            void AgeValidation()
            {
                DateTime todayDate = DateTime.Now;
                int timespan = (todayDate - birthdate_entry.Date).Days;

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

            UsernameValidation();
            MailValidation();
            PasswordValidation();
            PasswordConfirmationValidation();
            AgeValidation();

            //se tutti i campi sono rispettati la procedura ha successo
            if (emailOk && passwordOk && usernameOk && ageOk)
            {
                //test di aggiunta utente al db firebase
                /*
                var viewModel = (MyUsersViewModel)BindingContext;
                if (viewModel.AddUserCmd.CanExecute(null))
                {
                    viewModel.AddUserCmd.Execute(null);
                }
                */

                //try-catch di signup async
                try
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
                    //signup con email e password 
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Entry_Email.Text, Entry_Password.Text);
                    //invia email di verifica
                    await authProvider.SendEmailVerificationAsync(auth);
                    await authProvider.UpdateProfileAsync(auth.FirebaseToken, username, "");
                    //alert
                    await App.Current.MainPage.DisplayAlert("Success!", "Don't forget to verify your Email!", "OK");
                    await Navigation.PushAsync(new LoginPage());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Ops... Something went wrong", "Try to Sign Up again", "Ok");
                }

            }

        }

        //bottone che nasconde la password
        void HideButton_Clicked(System.Object sender, System.EventArgs e)
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

        //bottone che manda alla login page
        void Btn_SignUp_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());

        }
    }
}
