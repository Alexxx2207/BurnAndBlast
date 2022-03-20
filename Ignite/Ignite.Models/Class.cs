namespace Ignite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Class
    {
        public Class()
        {
            UsersClasses = new HashSet<UserClass>();
        }

        [Key]
        public string Guid { get; set; }

        [Required]
        public DateTime StartingDateTime { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        public decimal Price { get; set; }

        [Required]
        public int AllSeats { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string FitnessId { get; set; }

        public virtual Fitness Fitness { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserClass> UsersClasses { get; set; }
    }
}
