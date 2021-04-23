using System;
using Firebase.Auth;
using Newtonsoft.Json;
using SommeliAr.Models;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Menu
{
    public partial class MyHomePage : ContentPage

    {
        public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public MyHomePage()
        {
            InitializeComponent();
            //Toglie le righette di separazione delle entry della listview
            my_list_view.SeparatorVisibility = (SeparatorVisibility)1;
            GetProfileInformationAndRefreshToken();          
        }

        //colore violetto chiaro
        public Color myColor = Color.FromHex("#8b52ff");

        void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            User_name_lbl.Text = "";
            Welcome_lbl.Text = "";
            BindingContext = new MyFavoritesPageViewModel();
            my_list_view.SeparatorVisibility = 0;
           // History_btn.TextColor =Color.Black;
            History_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            History_btn.FontSize = 20;

            Favourites_btn.TextColor = myColor;
            Favourites_btn.FontSize = 30;
        }

        void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            User_name_lbl.Text = "";
            Welcome_lbl.Text = "";
            BindingContext = new MyHistoryPageViewModel();
            my_list_view.SeparatorVisibility = 0;
            //Favourites_btn.TextColor = Color.Black;
            Favourites_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            Favourites_btn.FontSize = 20;

            History_btn.TextColor = myColor;
            History_btn.FontSize = 30;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description, mydetails.Image));

        }

        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyLoginToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyLoginToken", JsonConvert.SerializeObject(RefreshedContent));
                //getting user info
                User_name_lbl.Text ="Hi, " + savedfirebaseauth.User.DisplayName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }

        }
    }
}