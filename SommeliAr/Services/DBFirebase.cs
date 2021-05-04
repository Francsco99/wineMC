using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using SommeliAr.Models;
using Xamarin.Essentials;

namespace SommeliAr.Services
{
    public class DBFirebase
    {
        FirebaseClient client;
        static AuthFirebase authServices = new AuthFirebase();

        private static DBFirebase instance;

        private DBFirebase()
        {
            client = new FirebaseClient("https://sommelier-ar-default-rtdb.europe-west1.firebasedatabase.app/");
        }


        public static DBFirebase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBFirebase();
                }
                return instance;
            }
        }

        //aggiunge un user al db realtime
        public async Task AddMyUser(string email, DateTime birthdate, string firebaseMail )
        {
            MyUser u = new MyUser() { Email = email, Birthdate = birthdate };
            await client
                .Child("Users")
                .Child(firebaseMail)
                .PutAsync(u);
        }

        //aggiunge un vino al db realtime
        public async Task AddMyWine(string name, string detail, string image, string description)
        {
            MyWineModel w = new MyWineModel() { Name = name, Detail = detail, Image = image, Description = description };
            await client
                .Child("MyWines")
                .Child(name)
                .PutAsync(w);
        }

        //restituisce la lista di tutti i vini
        public ObservableCollection<MyWineModel> GetAllWines()
        {
            var myWinesData = client
                .Child("MyWines")
                .AsObservable<MyWineModel>()
                .AsObservableCollection();
            return myWinesData;
        }


        //aggiunge un vino ai preferiti dell'utente corrente
        public async Task AddFavWine(string wineName, string firebaseMail)
        {
            await client
                .Child("Users")
                .Child(firebaseMail)
                .Child("favourites")
                .Child(wineName)
                .PutAsync(true);
        }

        //restituisce la lista dei vini preferiti
        public async Task<ObservableCollection<MyWineModel>> GetMyFavouriteWines(string firebaseMail)
        {
            var favouriteWines = new ObservableCollection<MyWineModel>();
            var wineNames = await client
                .Child("Users")
                .Child(firebaseMail)
                .Child("favourites")
                .OnceAsync<bool>();

            foreach (var v in wineNames)
            {
                var wines = await client
                    .Child("MyWines")
                    .OrderByKey()
                    .StartAt(v.Key)
                    .LimitToFirst(1)
                    .OnceAsync<MyWineModel>();

                foreach(var w in wines)
                {
                    favouriteWines.Add(w.Object);
                }
            }
            return favouriteWines;
        }
    }
}