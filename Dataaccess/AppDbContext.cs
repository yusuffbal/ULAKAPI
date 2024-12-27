using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = Dataaccess.Helpers.ConfigurationManager;

namespace Dataaccess
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connString = ConfigurationManager.Configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connString))
                    optionsBuilder.UseSqlServer(connString, options => options.EnableRetryOnFailure());
            }

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<MessageGroups> MessageGroups { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<PollOptions> PollOptions { get; set; }
        public DbSet<Polls> Polls { get; set; }
        public DbSet<PollVotes> PollVotes { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<UserTeams> UserTeams { get; set; }
        public DbSet<TaskLocation> TaskLocation { get; set; }





    }
}
