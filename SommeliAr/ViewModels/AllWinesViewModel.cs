using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SommeliAr.Models;
using SommeliAr.Services;

namespace SommeliAr.ViewModels
{
    public class AllWinesViewModel : BaseViewModel
    {
       public string Name { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        private DBFirebase services;

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
            services = new DBFirebase();
            MyWineList = services.GetMyWines();
            AddWineCommand = new Command(async () => await AddMyWineAsync(Name, Detail, Image, Description));
        }

        public async Task AddMyWineAsync(string Name, string Detail, string Image, string Birthdate)
        {
            await services.AddMyWine(Name, Detail, Image, Description);
        }

    }
}