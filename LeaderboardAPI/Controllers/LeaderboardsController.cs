using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaderboardAPI.Models;

namespace LeaderboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardContext _context;

        public LeaderboardController(LeaderboardContext context)
        {
            _context = context;
        }

        // GET: api/Leaderboard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leaderboard>>> GetLeaderboard()
        {
          if (_context.Leaderboard == null)
          {
              return NotFound();
          }
            return await _context.Leaderboard.Include(s=>s.user).Include(s=>s.game).OrderByDescending(s=>s.Score).ThenByDescending(s=> s.UpdatedAt).ToListAsync();
        }

        // GET: api/Leaderboard/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leaderboard>> GetLeaderboard(int id)
        {
          if (_context.Leaderboard == null)
          {
              return NotFound();
          }
            var leaderboard = await _context.Leaderboard.FindAsync(id);

            if (leaderboard == null)
            {
                return NotFound();
            }

            return leaderboard;
        }

        // PUT: api/Leaderboard/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaderboard(int id, Leaderboard leaderboard)
        {
            if (id != leaderboard.IdLeaderboard)
            {
                return BadRequest();
            }

            _context.Entry(leaderboard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaderboardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
      
        // POST: api/Leaderboard
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostLeaderboard([FromForm] Leaderboard leaderboard)
        {
            var response = new PostResponse();
            try
            {
              if (_context.Leaderboard == null)
              {
                  return Problem("Entity set 'LeaderboardContext.Leaderboard'  is null.");
              }
                var existingData = await _context.Leaderboard.FirstOrDefaultAsync(s => s.IdUser == leaderboard.IdUser && s.IdGame == leaderboard.IdGame);
                var user = await _context.User.FirstOrDefaultAsync(s => s.IdUser == leaderboard.IdUser);
                var game = await _context.Game.FirstOrDefaultAsync(s => s.IdGame == leaderboard.IdGame);

                if (existingData == null)
                {
                    var newData = new Leaderboard
                    {
                        IdUser = leaderboard.IdUser,
                        IdGame = leaderboard.IdGame,
                        Score = leaderboard.Score,
                        UpdatedAt = DateTime.Now
                    };
                    _context.Leaderboard.Add(newData);
                    response.Status = "success";
                    response.Data = newData;
                }
                else if (leaderboard.Score >= existingData.Score)
                {
                    existingData.Score = leaderboard.Score;
                    existingData.UpdatedAt = DateTime.Now;

                    response.Status = "success";
                    response.Data = existingData;
                }
                else if (existingData.Score >= leaderboard.Score)
                {
                    response.Status = "success";
                    response.Data = existingData;
                }
                else
                {
                    response.Status = "fail";
                };
                await _context.SaveChangesAsync();            
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = "fail";
                return StatusCode(500, response.Status);
            }
        }

        // DELETE: api/Leaderboard/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaderboard(int id)
        {
            if (_context.Leaderboard == null)
            {
                return NotFound();
            }
            var leaderboard = await _context.Leaderboard.FindAsync(id);
            if (leaderboard == null)
            {
                return NotFound();
            }

            _context.Leaderboard.Remove(leaderboard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaderboardExists(int id)
        {
            return (_context.Leaderboard?.Any(e => e.IdLeaderboard == id)).GetValueOrDefault();
        }
    }
}
