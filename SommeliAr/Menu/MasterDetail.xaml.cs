using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Views.Menu
{
    public partial class MasterDetail : TabbedPage
    {

        public MasterDetail()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SetPreferencesAndRefreshToken();
        }

        private void SetPreferencesAndRefreshToken()
        {
            AuthFirebase services = new AuthFirebase();
            User user;
            try
            {
                user = services.GetUserInfo();
                Preferences.Set("UserEmailFirebase", user.Email.Replace(".", "-").Replace("@", "-"));
            }
            catch
            {
                services.RefreshToken();
                user = services.GetUserInfo();
                Preferences.Set("UserEmailFirebase", user.Email.Replace(".", "-").Replace("@", "-"));
            }
        }

    }
}
