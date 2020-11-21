using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class Friendship
    {

        public string UserId { get; set; }
        public User User { get; set; }

        public string UserFriendId { get; set; }
        
        [Display(Name  = "Friend")]
        public User UserFriend { get; set; }

      



        
    }
}