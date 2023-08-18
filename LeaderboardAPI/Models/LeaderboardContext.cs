using Microsoft.EntityFrameworkCore;

namespace LeaderboardAPI.Models
{
    public class LeaderboardContext:DbContext
    {
        public LeaderboardContext(DbContextOptions<LeaderboardContext> options) : base(options) 
        { 

        }  
        public DbSet<User> User { get; set; }
        public DbSet<Leaderboard> Leaderboard { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}