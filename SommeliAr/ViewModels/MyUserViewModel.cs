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
        
        public MyUserViewModel()
        {
            AddUserCommand = new Command(async () =>  await AddMyUserAsync(Email));
        }

        public async Task AddMyUserAsync(string Email)
        {
            await DBFirebase.Instance.AddMyUser(Email, Preferences.Get("UserEmailFirebase", ""));
        }
        
    }
}
