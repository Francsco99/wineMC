using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using SommeliAr.Models;

namespace SommeliAr.Services
{
    public class DBFirebase
    {
        FirebaseClient client;

        public DBFirebase()
        {
            client = new FirebaseClient("https://sommelier-ar-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public ObservableCollection<MyUser> GetMyUser()
        {
            var myUserData = client
                .Child("MyUsers")
                .AsObservable<MyUser>()
                .AsObservableCollection();

                return myUserData;
        }

        public async Task AddMyUser(string username, string email, string password, DateTime birthdate)
        {
            MyUser u = new MyUser() { Username = username, Email = email, Password = password, Birthdate=birthdate};
            await client
                .Child("MyUsers")
                .PostAsync(u);
        }
    }
}
