using System;
using System.ComponentModel.DataAnnotations;

namespace Airport.DAL.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Flight Flight { get; set; }
    }
}
