using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using SommeliAr.Models;
using SommeliAr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.ViewModels
{
    public class MyFavoritesPageViewModel : BaseViewModel
    {
        private ObservableCollection<MyWineModel> _wineList = new ObservableCollection<MyWineModel>();
        
        public ObservableCollection<MyWineModel> WineList
        {
            get { return _wineList; }

            set
            {
                _wineList = value;
                OnPropertyChanged();
            }
        }

        public MyFavoritesPageViewModel()
        {
            
        }

        public async Task GetInfoAsync()
        {
            ObservableCollection<MyWineModel> result = await Task.Run(() => DBFirebase.Instance.GetMyFavouriteWines(Preferences.Get("UserEmailFirebase", "")));
            WineList = result;
        }
    }
}