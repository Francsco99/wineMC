using System;
using System.Collections.ObjectModel;
using SommeliAr.Models;

namespace SommeliAr.ViewModels
{
    public class MyHistoryPageViewModel
    {

        public ObservableCollection<MyListModel> WineList { get; set; }

        public MyHistoryPageViewModel()
        {

            WineList = new ObservableCollection<MyListModel>();
            WineList.Add(new MyListModel { Name = "Barolo", Image = "https://data.negoziodelvino.it/imgprodotto/barolo-docg_11068.jpg", Detail = "Prunotto", Ingredients = "Questo si che è un buon vino!" });
        }
    }
}