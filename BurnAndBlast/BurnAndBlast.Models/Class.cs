﻿namespace BurnAndBlast.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Class
    {
        public Class()
        {
            this.UsersClasses = new HashSet<UserClass>();
        }

        [Key]
        public string Guid { get; set; }

        [Required]
        public DateTime StartingDateTime { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [Required]
        public int AllSeats { get; set; }

        [Required]
        public bool IsSuspended { get; set; }

        [Required]
        public string FitnessId { get; set; }

        public virtual Fitness Fitness { get; set; }

        public virtual ICollection<UserClass> UsersClasses { get; set; }
    }
}
