using System;
using System.Collections.ObjectModel;
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

        public DBFirebase()
        {
            client = new FirebaseClient("https://sommelier-ar-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public ObservableCollection<MyWineModel> GetMyWines()
        {
            var myWinesData = client
                .Child("MyWines")
                .AsObservable<MyWineModel>()
                .AsObservableCollection();

            return myWinesData;
        }

        public ObservableCollection<MyUser> GetMyUsers()
        {
            var myUsersData = client
                .Child("MyUsers")
                .AsObservable<MyUser>()
                .AsObservableCollection();

            return myUsersData;
        }

        public async Task<Favourites> GetMyFavouriteWines()
        {
            var userMail = Preferences.Get("UserEmailFirebase", "");

            var favouriteWines = await client
                .Child("Users")
                .Child(userMail)
                .Child("favourites")
                .OrderByKey()
                .OnceAsync<Favourites>();

            foreach (var element in favouriteWines)
            {
                Console.WriteLine(element.Object.ToString());

                return element.Object;
            }
            return null;
        }

        public async Task AddMyWine(string name, string detail, string image, string description)
        {
            MyWineModel w = new MyWineModel() { Name = name, Detail = detail, Image = image, Description=description };
            await client
                .Child("MyWines")
                .PostAsync(w);
        }

        public async Task AddMyUser(string email)
        {
            MyUser u = new MyUser() {Email = email };
            var userMail = Preferences.Get("UserEmailFirebase", "");

            await client
                .Child("Users")
                .Child(userMail)
                .PutAsync(u);
        }

        public async Task AddFavouriteWine(string name)
        {
            var userMail = Preferences.Get("UserEmailFirebase","");
            var favouritesDB = await this.GetMyFavouriteWines();
            var favourites = new Favourites();
            if (favouritesDB!=null)
            {
                favourites = favouritesDB;   
            }

            favourites.addToFavourites(name);

            if (!userMail.Equals(""))
            {
                await client
                    .Child("Users")
                    .Child(userMail)
                    .PutAsync(favourites);
            }

            Console.WriteLine(userMail);

        }
    }
}