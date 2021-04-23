using System;
using System.Collections.ObjectModel;
using SommeliAr.Models;

namespace SommeliAr.ViewModels
{
    public class MyHistoryPageViewModel
    {

        public ObservableCollection<MyWineModel> WineList { get; set; }

        public MyHistoryPageViewModel()
        {

            WineList = new ObservableCollection<MyWineModel>();
            WineList.Add(new MyWineModel { Name = "Barolo", Image = "https://data.negoziodelvino.it/imgprodotto/barolo-docg_11068.jpg", Detail = "Prunotto", Description = "Questo si che è un buon vino!" });
        }
    }
}