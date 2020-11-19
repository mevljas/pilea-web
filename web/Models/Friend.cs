using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Friend
    {
        public int FriendID { get; set; }
        public User User { get; set; }

        // Foreign keys
        public int UserID { get; set; }
        
    }
}