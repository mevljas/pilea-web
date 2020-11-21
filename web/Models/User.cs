using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace web.Models
{
    public class User : IdentityUser 
    {

        public int UserID;
        public string nickname { get; set; }


        public ICollection<Friendship> FriendsOf { get; set; }
        public ICollection<Friendship> Friends { get; set; }




    }
}