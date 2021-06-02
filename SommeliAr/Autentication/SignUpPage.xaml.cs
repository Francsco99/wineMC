using System;
using System.Text.RegularExpressions;
using Firebase.Auth;
using SommeliAr.Models;
using SommeliAr.Services;
using SommeliAr.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MyUserViewModel();
        }

        bool MailValidation(string email)
        {
            //espressione regolare per verificare se la mail inserita ha una formattazione valida
            string emailPattern = "^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))\\z";

            bool emailOk;

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
                        Email_err_lbl.TextColor = Color.White;
                        break;

                    case false:
                        ErrorLabelText.Opacity = 0.7;
                        Email_err_lbl.TextColor = Color.Red;
                        break;
                }
            }
            else
            {
                ErrorLabelText.Opacity = 0.7;
                Email_err_lbl.TextColor = Color.Red;
                emailOk = false;
            }
            return emailOk;
        }

        bool PasswordConfirmationValidation(MyUser user)
        {
            bool passwordConfirmOk;

            if (!user.IsPasswordMatching(Entry_Password.Text, Entry_ConfirmPassword.Text))
            {
                ErrorPwdLabelText.Opacity = 0.7;
                passwordConfirmOk = false;
                Pwd_conf_err_lbl.TextColor = Color.Red;
            }
            else
            {
                ErrorPwdLabelText.Opacity = 0;
                passwordConfirmOk = true;
                Pwd_conf_err_lbl.TextColor = Color.White;
            }
            return passwordConfirmOk;
        }

        bool UsernameValidation(string username)
        {
            bool usernameOk;
            //Deve contenere tra i 6 e i 18 caratteri alfanumerici
            string userPattern = "[A-Za-z][A-Za-z0-9._]{6,18}";

            if (username != null)
            {
                if (Regex.IsMatch(username, userPattern))
                {
                    usernameOk = true;
                    User_err_lbl.TextColor = Color.White;
                    UserErrorIcon.Opacity = 0;
                }

                else
                {
                    usernameOk = false;
                    User_err_lbl.TextColor = Color.Red;
                    UserErrorIcon.Opacity = 0.7;
                }
            }
            else
            {
                usernameOk = false;
                User_err_lbl.TextColor = Color.Red;
                UserErrorIcon.Opacity = 0.7;
            }
            return usernameOk;
        }

        bool PasswordValidation(string pwd)
        {
            bool passwordOk;
            //tra 8-20 cifre, un carattere maiuscolo, un carattere minuscolo, un numero e un carattere speciale
            string passwordPattern = "(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}";

            if (pwd != null )
            {
                if (Regex.IsMatch(pwd, passwordPattern))
                {
                    passwordOk = true;
                    Pwd_err_lbl.TextColor = Color.White;
                    PwdErrorIcon.Opacity = 0;
                }

                else
                {
                    passwordOk = false;
                    Pwd_err_lbl.TextColor = Color.Red;
                    PwdErrorIcon.Opacity = 0.7;
                }
            }
            else
            {
                passwordOk = false;
                Pwd_err_lbl.TextColor = Color.Red;
                PwdErrorIcon.Opacity = 0.7;
            }
            return passwordOk;
        }

        bool AgeValidation()
        {
            bool ageOk;
            DateTime todayDate = DateTime.Now;
            int timespan = (todayDate - Birthdate_entry.Date).Days;

            if (timespan >= 6570)
            {
                ageOk = true;
                ErrorBirthLabelText.TextColor = Color.White;
                Bday_err_lbl.Opacity = 0;
            }

            else
            {
                ageOk = false;
                ErrorBirthLabelText.TextColor = Color.Red;
                Bday_err_lbl.Opacity = 0.7;

            }
            return ageOk;
        }

        //procedura di sign up
        async void Sign_up_btn_Clicked(object sender, EventArgs e)
        {
            MyUser user = new MyUser();
            string username = Entry_Username.Text;
            string emailLowerCase = Entry_Email.Text;
            if (Entry_Email.Text != null)
            {
                emailLowerCase = Entry_Email.Text.ToLower();
            }
            string pwd = Entry_Password.Text;
            
            bool isEmailOk = MailValidation(emailLowerCase);
            bool isPassConfOk = PasswordConfirmationValidation(user);
            bool isUsernameOk = UsernameValidation(username);
            bool isPassOk = PasswordValidation(pwd);
            bool isAgeOk = AgeValidation();
            //bool test = true;
            
            //se tutti i campi sono rispettati la procedura ha successos
            if (isEmailOk && isPassConfOk && isUsernameOk && isPassOk && isAgeOk)
            {
                try
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(AuthFirebase.Instance.GetKey()));
                    //signup con email e password 
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Entry_Email.Text, Entry_Password.Text);
                    //invia email di verifica
                    await authProvider.SendEmailVerificationAsync(auth);
                    await authProvider.UpdateProfileAsync(auth.FirebaseToken, username, "");
                    Preferences.Set("UserEmailFirebase", emailLowerCase.Replace(".", "-").Replace("@", "-at-"));
                    //alert
                    await App.Current.MainPage.DisplayAlert("Success!", "Don't forget to verify your Email.", "Ok");

                    //aggiunto utente nel database
                    var viewModel = (MyUserViewModel)BindingContext;
                    if (viewModel.AddUserCommand.CanExecute(null))
                    {
                        viewModel.AddUserCommand.Execute(null);
                    }

                    //navigazione alla pagina di login
                    await Navigation.PushAsync(new LoginPage());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Ops... Something went wrong.", "Try to Sign Up again.", "Ok");
                }

            }

        }

        //bottone che nasconde la password
        void Hide_pwd_btn_Clicked(System.Object sender, System.EventArgs e)
        {

            if (Entry_Password.IsPassword == true)
            {
                Entry_Password.IsPassword = false;
                Entry_ConfirmPassword.IsPassword = false;
                Hide_pwd_btn.Source = "eye_64.png";
            }
            else
            {
                Entry_Password.IsPassword = true;
                Entry_ConfirmPassword.IsPassword = true;
                Hide_pwd_btn.Source = "closed_eye_64.png";
            }
        }

        //bottone che manda alla sign in page
        void Sign_in_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
    }
}
