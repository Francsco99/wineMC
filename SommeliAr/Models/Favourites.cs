using System;
using System.Collections.Generic;

namespace SommeliAr
{
    public class Favourites
    {
        public List<string> favourites { get; set; }

        public Favourites()
        {
            favourites = new List<string>();
        }

        public void addToFavourites(string wineName)
        {
            favourites.Add(wineName);
        }
    }
}
