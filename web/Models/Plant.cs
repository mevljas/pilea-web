using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace web.Models
{
    public class Plant
    {
        public int PlantID { get; set; }
        public string Name { get; set; }
        // Show this text when there is no entry.
        [DisplayFormat(NullDisplayText = "No description")]
        public string? Description { get; set; }
        [DisplayFormat(NullDisplayText = "No note")]
        public string? Note { get; set; }
        public byte[]? image { get; set; }
        [Display(Name = "Days between watering")]
        public int DaysBetweenWatering { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last watering date")]
        public DateTime LastWateredDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Next watering date")]
        public DateTime NextWateredDate { get; set; }

        [DisplayFormat(NullDisplayText = "No category")]
        public Category? Category { get; set; }

        public int CategoryID { get; set; }

        [DisplayFormat(NullDisplayText = "No location")]
        public Location? Location { get; set; }
        public int LocationID { get; set; }
        

        public User User { get; set; }

        

    }
}