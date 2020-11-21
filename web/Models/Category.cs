using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{

    public class Category
    {
        public int CategoryID { get; set; }

        [Display(Name  = "Category name")]
        public String PlantCategory { get; set; }
        public ICollection<Plant> Plants { get; set; }
    }
}