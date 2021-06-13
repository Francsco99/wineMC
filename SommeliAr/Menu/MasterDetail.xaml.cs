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
                user = services.GetUserFromDB();
                Preferences.Set("UserEmailFirebase", user.Email.Replace(".", "-").Replace("@", "-at-"));
            }
            catch
            {
                services.RefreshToken();
                user = services.GetUserFromDB();
                Preferences.Set("UserEmailFirebase", user.Email.Replace(".", "-").Replace("@", "-at-"));
            }
        }

        string currentPageName = "";  // VEDIAMO SE IMPLEMENTARLO IN QUESTO MODO
       /* protected override void OnCurrentPageChanged()
        {
           base.OnCurrentPageChanged();

           currentPageName = CurrentPage.Title;

            if(CurrentPage.Title == "ScanPage")
            {
                
            }
        }*/
       
    }
}
