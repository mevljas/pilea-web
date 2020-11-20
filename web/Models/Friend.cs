using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Friend
    {
        public int FriendID { get; set; }
        public LocalUser LocalUser { get; set; }

        // Foreign keys
        public int LocalUserID { get; set; }
        
    }
}