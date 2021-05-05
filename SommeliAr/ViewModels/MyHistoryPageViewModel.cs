using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using SommeliAr.Models;
using SommeliAr.Services;
using Xamarin.Essentials;

namespace SommeliAr.ViewModels
{
    public class MyHistoryPageViewModel : BaseViewModel
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

        public MyHistoryPageViewModel()
        {

        }

        public async Task GetInfoAsync()
        {
            ObservableCollection<MyWineModel> result = await Task.Run(() => DBFirebase.Instance.GetMyHistoryWines(Preferences.Get("UserEmailFirebase", "")));
            WineList = result;
        }
    }
}