using System;
using System.Collections.ObjectModel;
using SommeliAr.Models;

namespace SommeliAr.ViewModels
{
    public class MyFavoritesPageViewModel
    {

        public ObservableCollection<MyWineModel> WineList { get; set; }

        public MyFavoritesPageViewModel()
        {

            WineList = new ObservableCollection<MyWineModel>();
            WineList.Add(new MyWineModel { Name = "San Crispino", Image = "https://content.dambros.it/uploads/2018/01/18180734/0000091987.jpg", Detail = "Il nostro vino! Cit.", Description = "(Quasi certamente) Uva" });
            WineList.Add(new MyWineModel { Name = "Tavernello", Image = "https://www.cicalia.com/it/img/imgproducts/17582/l_17582.jpg", Detail = "È un succo de sulfiti! Cit.", Description = "(Forse) Uva" });
        }
    }
}