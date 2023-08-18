using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardAPI.Models
{
    public class Leaderboard
    {
        [Key]
        public int IdLeaderboard { get; set; }
        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User? user { get; set; }
        public int IdGame { get; set; }
        [ForeignKey("IdGame")]
        public Game? game { get; set; }
        public int Score { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PostResponse
    {
        public string Status { get; set; }
        public Leaderboard Data { get; set; }
    }
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Game
    {
        [Key]
        public int IdGame { get; set; }
        public string Name { get; set; }
    }
}
