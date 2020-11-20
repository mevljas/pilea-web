using System;
using System.Collections.Generic;


namespace web.Models
{
    public class Plant
    {
        public int PlantID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public byte[]? image { get; set; }
        public int DaysBetweenWatering { get; set; }
        public DateTime LastWateredDate { get; set; }

        public DateTime NextWateredDate { get; set; }

        public Category? Category { get; set; }
        public int CategoryID { get; set; }
        // public ICollection<Waters> Waters { get; set; }

        public ApplicationUser User { get; set; }

    }
}