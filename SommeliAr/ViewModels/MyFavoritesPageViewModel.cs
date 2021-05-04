using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SommeliAr.Models;
using SommeliAr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SommeliAr.ViewModels
{
    public class MyFavoritesPageViewModel
    {
        public ObservableCollection<MyWineModel> WineList { get; set; }


        public MyFavoritesPageViewModel()
        {
            WineList = new ObservableCollection<MyWineModel>();
  
        }

       
    }
}