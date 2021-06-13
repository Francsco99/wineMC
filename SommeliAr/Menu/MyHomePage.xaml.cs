using System;
using System.Collections.Generic;
using Firebase.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SommeliAr.Models;
using SommeliAr.Services;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.Menu
{

    public partial class MyHomePage : ContentPage
    {
        private void ShowWelcomeLabels()
        {
            Welcome_msg.IsVisible = true;

        }

        private void ResetView()
        {
            History_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            Favourites_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            History_btn.FontSize = 25;
            Favourites_btn.FontSize = 25;
            ShowWelcomeLabels();
        }

        private void GetUserInformationAndRefreshToken()
        {
            User user = AuthFirebase.Instance.GetUserFromDB();
            try
            {
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
            catch
            {
                AuthFirebase.Instance.RefreshToken();
                User_name_lbl.Text = "Hi, " + user.DisplayName;
            }
        }

        public MyHomePage()
        {
            InitializeComponent();
            
            /*Toglie le righette di separazione delle entry della listview*/
            my_list_view.SeparatorVisibility = (SeparatorVisibility)1;

            GetUserInformationAndRefreshToken();
            ResetView();
            SetUpList();
            GetRandomWine();
        }
        private List<string> wineNames = new List<string>();
        private bool favClicked = false;
        private bool histClicked = false;
        private static Color violetto = Color.FromHex("#8b52ff");

        private MyWineModel randWine;

        private void DeleteWelcomeLabels()
        {
            Welcome_msg.IsVisible = false;
        }

        private void SetUpList()
        {
            this.wineNames.Add("Arpatin Barbaresco");
            this.wineNames.Add("Ascheri Barolo");
            this.wineNames.Add("Bolla Amarone della Valpolicella");
            this.wineNames.Add("Borgo di Fradis Collio Friulano");
            this.wineNames.Add("Masi Campofiorin");
            this.wineNames.Add("Ribolla Schiopetto");
            this.wineNames.Add("Sartori Valpolicella Classico");
            this.wineNames.Add("Tavernello");
            this.wineNames.Add("Terre Alte");
            this.wineNames.Add("Villa Mondi Amarone della Valpolicella Classico");
            this.wineNames.Add("Villa Mondi Valpolicella Ripasso Classico Superiore");
            this.wineNames.Add("Volpe Pasini Sauvignon");
            this.wineNames.Add("Volpe Pasini Togliano Merlot");
        }

        private async void GetRandomWine()
        {
            await bottle_img.TranslateTo(130, 0, 250);

            var rnd = new Random();
            int r = rnd.Next(wineNames.Count);

            bottle_img.IsVisible = false;
            bottle_title.IsVisible = false;
            
            var wine = await DBFirebase.Instance.GetWineFromName(wineNames[r]);
            this.randWine = wine;
            bottle_title.Text = wine.Name;
            bottle_img.Source = wine.Image;
            bottle_img.IsVisible = true;
            bottle_title.IsVisible = true;
            
            await bottle_img.TranslateTo(-130, 0,0);
            await bottle_img.TranslateTo(0, 0, 600, Easing.Linear);
        }
        
        async void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            this.favClicked = true;
            this.histClicked = false;
            DeleteWelcomeLabels();
            my_list_view.IsVisible = false;
            //setting del BindingContext
            MyFavoritesPageViewModel fav = new MyFavoritesPageViewModel();
            Loading.IsVisible = true;
            await fav.GetInfoAsync();
            Loading.IsVisible = false;
            my_list_view.IsVisible = true;
            BindingContext = fav;
            my_list_view.SeparatorVisibility = 0;

            //setting del colore in base al tema del dispositivo
            History_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            History_btn.FontSize = 25;
            Favourites_btn.TextColor = violetto;
            Favourites_btn.FontSize = 28;
        }

        async void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            this.favClicked = false;
            this.histClicked = true;
            DeleteWelcomeLabels();
            my_list_view.IsVisible = false;
            //setting del BindingContext
            MyHistoryPageViewModel his = new MyHistoryPageViewModel();
            Loading.IsVisible = true;
            await his.GetInfoAsync();
            Loading.IsVisible = false;
            my_list_view.IsVisible = true;
            BindingContext = his;
            my_list_view.SeparatorVisibility = 0;

            //setting del colore in base al tema del dispositivo
            Favourites_btn.SetAppThemeColor(Label.TextColorProperty, Color.Black, Color.White);
            Favourites_btn.FontSize = 25;
            History_btn.TextColor = violetto;
            History_btn.FontSize = 28;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            if (histClicked)
            {
                await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description,mydetails.SensorialNotes,mydetails.ProductionArea,mydetails.Dishes, mydetails.Image, mydetails.Rating));
            }
            else if (favClicked)
            {
                await Navigation.PushAsync(new MyFavouritePageDetail(mydetails.Name, mydetails.Description,mydetails.SensorialNotes,mydetails.ProductionArea,mydetails.Dishes, mydetails.Image, mydetails.Rating));
            }
        }

       public async void My_list_view_Refreshing(System.Object sender, System.EventArgs e)
        {
            if (this.favClicked)
            {
                //setting del BindingContext
                MyFavoritesPageViewModel fav = new MyFavoritesPageViewModel();
                await fav.GetInfoAsync();
                BindingContext = fav;
                my_list_view.EndRefresh();
            }

            else if (this.histClicked)
            {
                //setting del BindingContext
                MyHistoryPageViewModel his = new MyHistoryPageViewModel();
                await his.GetInfoAsync();
                BindingContext = his;
                my_list_view.EndRefresh();
            }
        }

        void Bottle_img_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MyListPageDetail(randWine.Name, randWine.Description, randWine.SensorialNotes, randWine.ProductionArea, randWine.Dishes, randWine.Image, randWine.Rating));
        }

        void Refresh_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            this.GetRandomWine();
           
            Refresh_btn.PlayAnimation();
        }
    }
}
