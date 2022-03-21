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
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime StartingDateTime { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        public decimal Price { get; set; }

        [Required]
        public int AllSeats { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserClass> UsersClasses { get; set; }
    }
}
