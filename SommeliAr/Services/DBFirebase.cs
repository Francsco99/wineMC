using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using SommeliAr.Models;

namespace SommeliAr.Services
{
    public class DBFirebase
    {
        FirebaseClient client;
        static AuthFirebase authServices = new AuthFirebase();

        private static DBFirebase instance;

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

        private DBFirebase()
        {
            client = new FirebaseClient("https://sommelier-ar-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        //aggiunge un user al db realtime
        public async Task AddMyUser(string email, DateTime birthdate, string firebaseMail )
        {
            MyUser u = new MyUser() { Email = email, Birthdate = birthdate, };
            await client
                .Child("Users")
                .Child(firebaseMail)
                .PutAsync(u);
        }

        //aggiunge un vino al db realtime
        public async Task AddMyWine(string name, string detail, string image, string description, string rating)
        {
            MyWineModel w = new MyWineModel() { Name = name, Detail = detail, Image = image, Description = description, Rating=rating };
            await client
                .Child("AllWines")
                .Child(name)
                .PutAsync(w);
        }

        //restituisce la lista di tutti i vini
        public ObservableCollection<MyWineModel> GetAllWines()
        {
            var myWinesData = client
                .Child("AllWines")
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
                    .Child("AllWines")
                    .OrderByKey()
                    .StartAt(v.Key)
                    .LimitToFirst(1)
                    .OnceAsync<MyWineModel>();

                foreach (var w in wines)
                {
                    favouriteWines.Add(w.Object);
                }
            }
            return favouriteWines;
        }

        //aggiunta vini alla cronologia sul db
        public async Task AddHistoryWines(string wineName, string firebaseMail)
        {
                await client
                    .Child("Users")
                    .Child(firebaseMail)
                    .Child("history")
                    .Child(wineName)
                    .PutAsync(true);
        }

        //restituisce la lista della cronologia
        public async Task<ObservableCollection<MyWineModel>> GetMyHistoryWines(string firebaseMail)
        {
            var historyWines = new ObservableCollection<MyWineModel>();
            var wineNames = await client
                .Child("Users")
                .Child(firebaseMail)
                .Child("history")
                .OnceAsync<bool>();

            foreach (var v in wineNames)
            {
                var wines = await client
                    .Child("AllWines")
                    .OrderByKey()
                    .StartAt(v.Key)
                    .LimitToFirst(1)
                    .OnceAsync<MyWineModel>();

                foreach (var w in wines)
                {
                    historyWines.Add(w.Object);
                }
            }
            return historyWines;
        }

        public async Task<ObservableCollection<MyWineModel>> GetResultWines(string listaVini)
        {
            List<string> wineNames = listaVini.Split(',').ToList();

            var resultList = new ObservableCollection<MyWineModel>();
    
            foreach (var v in wineNames)
            {
                var wines = await client
                    .Child("AllWines")
                    .OrderByKey()
                    .StartAt(v)
                    .LimitToFirst(1)
                    .OnceAsync<MyWineModel>();

                foreach (var w in wines)
                {
                    resultList.Add(w.Object);
                }
            }
            return resultList;
        }


        public async Task DeleteFavWine(string wineName, string firebaseMail)
        {
            await client
                .Child("Users")
                .Child(firebaseMail)
                .Child("favourites")
                .Child(wineName)
                .DeleteAsync();
        }
    }
}