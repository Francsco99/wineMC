using System;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace SommeliAr.Services
{
    public class AuthFirebase

    {
       public string WebAPIKey = "AIzaSyB8W5Hq33E8rGn0Bn1CFf3-mzZDydeJSyA";

        public string GetKey()
        {
            return this.WebAPIKey;
        }

        private static AuthFirebase instance;

        public static AuthFirebase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthFirebase();
                }
                return instance;
            }
        }

        async public void RefreshToken()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyLoginToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                RefreshLoginToken(RefreshedContent);           

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }
        }

        public void SetLoginToken(string content)
        {
            Preferences.Set("MyLoginToken", content);
        }

        public void RefreshLoginToken(FirebaseAuth auth)
        {
            Preferences.Set("MyLoginToken", JsonConvert.SerializeObject(auth));
        }

        public User GetUserFromDB()
        {
            return JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyLoginToken", "")).User;      
        }
    }
}
