using System;
using System.Collections.Generic;

namespace web.Models
{
    public enum PlantCategory
    {
        Category1, Category2
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public PlantCategory PlantCategory { get; set; }
        public ICollection<Plant> Plants { get; set; }
    }
}