using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Friend
    {
        public int FriendID { get; set; }

        public ApplicationUser User { get; set; }
        
    }
}