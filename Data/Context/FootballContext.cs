using ASPDotnetFC.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPDotnetFC.Data.Context
{
    public class FootballContext : DbContext
    {
        public FootballContext(DbContextOptions<FootballContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubCompetition> ClubCompetitions { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Player> Players { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubCompetition>().HasKey(cl => new { cl.ClubId, cl.CompetitionId });

            modelBuilder.Entity<ClubCompetition>()
            .HasOne(c => c.Club)
            .WithMany(cl => cl.ClubCompetitions)
            .HasForeignKey(c => c.ClubId);

            modelBuilder.Entity<ClubCompetition>()
             .HasOne(l => l.Competition)
              .WithMany(cl => cl.ClubCompetitions)
              .HasForeignKey(l => l.CompetitionId);

        }


    }
}
