using System;
namespace SommeliAr.Models
{
    public class MyUser
    {

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

       // public DateTime Birthdate { get; set; }

        public MyUser() { }

        public MyUser(string Email)
        {
            this.Email = Email;
        }

        public bool IsPasswordMatching(string p1, string p2)
        {
            if (p1 != null && p2 != null)
                return p1.Equals(p2);

            else
                return false;
        }
    }

}