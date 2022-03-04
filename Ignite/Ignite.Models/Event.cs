namespace Ignite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Event
    {
        public Event()
        {
            UsersEvents = new HashSet<UserEvent>();
        }

        [Key]
        public string Guid { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsSuspended { get; set; }

        public DateTime StartingDateTime { get; set; }

        public virtual ICollection<UserEvent> UsersEvents { get; set; }
    }
}
