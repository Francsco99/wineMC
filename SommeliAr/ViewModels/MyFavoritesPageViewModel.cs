using System;
using System.Collections.ObjectModel;
using SommeliAr.Models;

namespace SommeliAr.ViewModels
{
    public class MyFavoritesPageViewModel
    {

        public ObservableCollection<MyListModel> WineList { get; set; }

        public MyFavoritesPageViewModel()
        {

            WineList = new ObservableCollection<MyListModel>();
            WineList.Add(new MyListModel { Name = "San Crispino", Image = "https://content.dambros.it/uploads/2018/01/18180734/0000091987.jpg", Detail = "Il nostro vino! Cit.", Ingredients = "(Quasi certamente) Uva" });
            WineList.Add(new MyListModel { Name = "Tavernello", Image = "https://www.cicalia.com/it/img/imgproducts/17582/l_17582.jpg", Detail = "È un succo de sulfiti! Cit.", Ingredients = "(Forse) Uva" });
        }
    }
}