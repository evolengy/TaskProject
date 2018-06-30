using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Complication> Complications { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Atribute> Atributes { get; set; }
        public DbSet<Repeat> Repeats { get; set; }
        public DbSet<Health> Healths { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Aligment> Aligments { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<UserMood> UserMoods { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserReward> UserRewards { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Goal>()
            //    .HasOne<Skill>(g => g.Skill)
            //    .WithMany(s => s.Goals)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.Entity<Catalog>()
            //    .HasOne<ApplicationUser>(c => c.User)
            //    .WithMany(u => u.Catalogs)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Health>()
            //    .HasOne<ApplicationUser>(h => h.User)
            //    .WithOne(u => u.Health)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Atribute>()
            //    .HasOne<ApplicationUser>(a => a.User)
            //    .WithMany(u => u.Atributes)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Skill>()
            //    .HasOne<ApplicationUser>(s => s.User)
            //    .WithMany(u => u.Skills)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<UserMood>()
            //  .HasOne<ApplicationUser>(s => s.User)
            //  .WithMany(u => u.UserMoods)
            //  .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Note>()
            //  .HasOne<ApplicationUser>(n => n.User)
            //  .WithMany(u => u.Notes)
            //  .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Notification>()
            //  .HasOne<ApplicationUser>(n => n.User)
            //  .WithMany(u => u.Notifications)
            //  .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
