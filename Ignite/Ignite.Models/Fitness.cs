﻿namespace Ignite.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Fitness
    {
        public Fitness()
        {
            Classes = new HashSet<Class>();
        }

        [Key]
        public string Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
