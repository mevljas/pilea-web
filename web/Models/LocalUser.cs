using System;
using System.Collections.Generic;

namespace web.Models
{
    public class LocalUser
    {
        public int LocalUserID { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public ICollection<Plant> Plants { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Friend> Friends { get; set; }


        // public ICollection<Waters> Waters { get; set; }
    }
}