using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SommeliAr.Models;
using SommeliAr.Services;
using Xamarin.Essentials;

namespace SommeliAr.ViewModels
{
    public class MyUserViewModel : BaseViewModel
    {
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }

        private DBFirebase services;

        public Command AddUserCommand { get; set; }

        /*
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
        */
        /*
        public MyUserViewModel()
        {
            // MyUsersList = services.GetMyUsers();
            AddUserCommand = new Command(async () =>  await AddMyUserAsync(Email, Birthdate));
        }

        public async Task AddMyUserAsync(string Email, DateTime Birthdate)
        {
            string firebaseMail = Preferences.Get("UserEmailFirebase", "");
            await DBFirebase.Instance.AddMyUser(Email, Birthdate, firebaseMail);
        }
        */
    }
}
