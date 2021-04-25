using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SommeliAr.Models;
using SommeliAr.Services;

namespace SommeliAr.ViewModels
{
    public class AllUsersViewModel : BaseViewModel
    {
        public string Email { get; set; }

        private DBFirebase services;

        public Command AddUserCommand { get; set; }

        private ObservableCollection<MyUser> _myUsers = new ObservableCollection<MyUser>();
        public ObservableCollection<MyUser> MyUsersList
        {
            get { return _myUsers; }

            set
            {
                _myUsers = value;
                OnPropertyChanged();

            }
        }

        public AllUsersViewModel()
        {
            services = new DBFirebase();
            MyUsersList = services.GetMyUsers();
            AddUserCommand = new Command(async () => await AddMyUserAsync(Email));
        }

        public async Task AddMyUserAsync(string Email)
        {
            await services.AddMyUser(Email);
        }

    }
}