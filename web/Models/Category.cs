using System;
using System.Collections.Generic;

namespace web.Models
{

    public class Category
    {
        public int CategoryID { get; set; }
        public String PlantCategory { get; set; }
        public ICollection<Plant> Plants { get; set; }
    }
}