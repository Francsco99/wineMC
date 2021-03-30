using System;
namespace SommeliAr.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User() { }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public bool checkInformation()
        {
            return (!this.Username.Equals("") && !this.Password.Equals(""));
             
        }
    }
}
