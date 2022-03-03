// ReSharper disable VirtualMemberCallInConstructor
namespace BurnAndBlast.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.UsersSubscriptions = new HashSet<UserSubscription>();
            this.UsersEvents = new HashSet<UserEvent>();
            this.UsersClasses = new HashSet<UserClass>();
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
