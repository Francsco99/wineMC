using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using SommeliAr.Models;
using SommeliAr.Services;
using Xamarin.Essentials;

namespace SommeliAr.ViewModels
{
    public class MyResultListViewModel : BaseViewModel
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

        public MyResultListViewModel()
        {
        }


        public async Task GetInfoAsync()
        {
            ObservableCollection<MyWineModel> result = await Task.Run(() => DBFirebase.Instance.GetResultWines(Preferences.Get("ResultList","")));
            WineList = result;
        }
    }
}
