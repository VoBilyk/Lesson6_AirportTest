using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airport.DAL.Entities
{
    public class Crew
    {
        public Guid Id { get; set; }

        
        public virtual Pilot Pilot { get; set; }

        
        [MinLength(1, ErrorMessage = "Crew can`t have less than 1 stewardess")]
        public virtual ICollection<Stewardess> Stewardesses { get; set; }

        public virtual ICollection<Departure> Departures { get; set; }
    }
}
