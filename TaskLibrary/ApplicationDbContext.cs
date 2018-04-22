using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskLibrary;

namespace TaskLibrary
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Complication> Complications { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<Attainment> Attainments { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<CharacterAtribute> CharacterAtributes { get; set; }
        public DbSet<Atribute> Atributes { get; set; }
        public DbSet<Repeat> Repeats { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionsCondition> MissionsConditions { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildsReputation> GuildsReputations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
