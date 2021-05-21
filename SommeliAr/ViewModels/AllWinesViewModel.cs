using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SommeliAr.Models;
using SommeliAr.Services;
using Xamarin.Essentials;

namespace SommeliAr.ViewModels
{
    public class AllWinesViewModel : BaseViewModel
    {
       public string Name { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string SensorialNotes { get; set; }
        public string ProductionArea { get; set; }
        public string Dishes { get; set; }
        public string Rating { get; set; }

        public Command AddWineCommand { get; set; }

        private ObservableCollection<MyWineModel> _myWines = new ObservableCollection<MyWineModel>();
        public ObservableCollection<MyWineModel> MyWineList
        {
            get { return _myWines; }

            set
            {
                _myWines = value;
                OnPropertyChanged();
            }
        }
        
        public AllWinesViewModel()
        {
            MyWineList = DBFirebase.Instance.GetAllWines();
            AddWineCommand = new Command(async () => await AddMyWineAsync(Name, Detail, Image, Description,SensorialNotes,ProductionArea,Dishes, Rating));
        }

        public async Task AddMyWineAsync(string Name, string Detail, string Image, string Description, string SensorialNotes,string ProductionArea, string Dishes, string Rating)
        {
            if(Name!=null && Detail!= null && Image!=null && Description != null && Rating != null)
            {
                await DBFirebase.Instance.AddMyWine(Name, Detail, Image, Description,SensorialNotes,ProductionArea,Dishes, Rating);
            }
        }
        
    }
}