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

        public ObservableCollection<MyWineModel> GetMyWines()
        {
            var myWinesData = client
                .Child("MyWines")
                .AsObservable<MyWineModel>()
                .AsObservableCollection();

            return myWinesData;
        }

        public async Task AddMyWine(string name, string detail, string image, string description)
        {
            MyWineModel w = new MyWineModel() { Name = name, Detail = detail, Image = image, Description=description };
            await client
                .Child("MyWines")
                .PostAsync(w);
        }
    }
}