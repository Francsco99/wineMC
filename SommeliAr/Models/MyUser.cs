using System;
namespace SommeliAr.Models
{
    public class MyUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public MyUser() { }

        public MyUser(string Username, string Password, string Email)
        {
            this.Username = Username;
            this.Password = Password;
            this.Email = Email;

        }

        public bool CheckInformation()
        {
            return (!this.Username.Equals("") && !this.Password.Equals(""));

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