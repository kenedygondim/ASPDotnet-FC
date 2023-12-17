using ASPDotnetFC.Data.Context;
using ASPDotnetFC.Models;
using aspdotnetfc_api.Interfaces;

namespace aspdotnetfc_api.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly FootballContext _context;

        public PlayerRepository(FootballContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //GET
        public ICollection<Player> GetPlayers()
        {
            var players = _context.Players.ToList();
            return players;
        }

        public Player GetPlayerById(int id)
        {
            return _context.Players.Where(s => s.Id == id).FirstOrDefault();
        }

        public Player GetPlayerByName(string nickname)
        {
            var player = _context.Players.Where(s => s.Nickname == nickname).FirstOrDefault();
            return player;
        }

        //POST
        public bool CreatePlayer(Player player)
        {
            _context.Add(player);
            return Save();
        }

        //PUT
        public bool UpdatePlayer(Player player)
        {
            _context.Update(player);
            return Save();
        }

        //DELETE
        public bool DeletePlayer(Player player)
        {
            _context.Remove(player);
            return Save();
        }

        //SAVE
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool AssociatePlayerClub(Player player, Club club)
        {
            club.TopScorer = player;
            return Save();
        }
    }
}
