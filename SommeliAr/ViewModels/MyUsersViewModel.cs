using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SommeliAr.Models;
using SommeliAr.Services;

namespace SommeliAr.ViewModels
{
    public class MyUsersViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private DBFirebase services;

        public Command AddUserCmd { get; set; }

        private ObservableCollection<MyUser> _myUsers = new ObservableCollection<MyUser>();
        public ObservableCollection<MyUser> MyUsers
        {
            get { return _myUsers; }

            set
            {
                _myUsers = value;
                OnPropertyChanged();

            }
        }

        public MyUsersViewModel()
        {
            services = new DBFirebase();
            MyUsers = services.GetMyUser();
            AddUserCmd = new Command(async () => await AddMyUserAsync(Username,Email,Password));
        }
        
        public async Task AddMyUserAsync(string Username, string Email, string Password)
        {
            await services.AddMyUser(Username, Email, Password);
        }

    }
}
