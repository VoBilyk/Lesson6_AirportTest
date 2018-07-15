using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport.DAL.Entities
{
    public class Aeroplane
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Aeroplane name must be not less than 3 symbols")]
        public string Name { get; set; }

        [Required]
        public AeroplaneType AeroplaneType { get; set; }

        [Required]
        public long LifeTimeHourses { get; set; }

        public virtual ICollection<Departure> Departures { get; set; }

        [NotMapped]
        public TimeSpan LifetimeFullForm {
            get
            {
                return LifetimeFullForm;
            }

            set
            {
                value = TimeSpan.FromHours(LifeTimeHourses);
            }
        }
    }
}
