using System;
using System.Collections.Generic;

namespace web.Models
{
    public class Location
    {
        public int LocationID { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
        public ICollection<Plant> Plants { get; set; }

        public ApplicationUser User { get; set; }




    }
}