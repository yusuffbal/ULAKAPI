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
                {
                    optionsBuilder.UseSqlServer(connString + ";TrustServerCertificate=true;", options => options.EnableRetryOnFailure());
                }
            }

            base.OnConfiguring(optionsBuilder);
        }


        // DbSet'ler
        public DbSet<MessageGroup> MessageGroups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollVote> PollVotes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<TaskLocation> TaskLocations { get; set; }

       
    }
}
