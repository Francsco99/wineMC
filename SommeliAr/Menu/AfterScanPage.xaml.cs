﻿using SommeliAr.Menu;
using SommeliAr.Models;
using SommeliAr.Services;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SommeliAr.Menu
{
    public partial class AfterScanPage : ContentPage
    {
        
         public AfterScanPage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            }
                InitializeComponent();
            //Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            //setting del BindingContext
        }
 


        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description,mydetails.SensorialNotes,mydetails.ProductionArea,mydetails.Dishes, mydetails.Image, mydetails.Rating));
        }

       async void Result_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            MyResultListViewModel res = new MyResultListViewModel();
            await res.GetInfoAsync();
            BindingContext = res;
        }
    }
}
