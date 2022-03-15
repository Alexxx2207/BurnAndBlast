using Ignite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ignite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Class> Classes { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Fitness> Fitnesses { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<UserClass> UsersClasses { get; set; }

        public DbSet<UserEvent> UsersEvents { get; set; }

        public DbSet<UserSubscription> UsersSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserClass>().HasKey(k => new { k.UserId, k.ClassId });
            builder.Entity<UserEvent>().HasKey(k => new { k.UserId, k.EventId });
            builder.Entity<UserSubscription>().HasKey(k => new { k.UserId, k.SubscriptionId });

            builder.Entity<Subscription>().Property(p => p.Price).HasColumnType("decimal").HasPrecision(38, 5);
        }
    }
}