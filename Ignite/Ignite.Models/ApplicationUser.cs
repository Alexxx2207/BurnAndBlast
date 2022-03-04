// ReSharper disable VirtualMemberCallInConstructor

namespace Ignite.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
            UsersSubscriptions = new HashSet<UserSubscription>();
            UsersEvents = new HashSet<UserEvent>();
            UsersClasses = new HashSet<UserClass>();
        }

        public bool IsDeleted { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<UserSubscription> UsersSubscriptions { get; set; }

        public virtual ICollection<UserEvent> UsersEvents { get; set; }

        public virtual ICollection<UserClass> UsersClasses { get; set; }
    }
}
