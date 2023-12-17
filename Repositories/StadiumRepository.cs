using ASPDotnetFC.Data.Context;
using ASPDotnetFC.Models;
using aspdotnetfc_api.Interfaces;

namespace aspdotnetfc_api.Repositories
{
    public class StadiumRepository : IStadiumRepository
    {
        private readonly FootballContext _context;

        public StadiumRepository(FootballContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //GET
        public ICollection<Stadium> GetStadiums()
        {
            var stadiums = _context.Stadiums.ToList();
            return stadiums;
        }


        public Stadium GetStadiumById(int id)
        {
            return _context.Stadiums.Where(s => s.Id == id).FirstOrDefault();
        }

        public Stadium GetStadiumByName(string name)
        {
            var stadium = _context.Stadiums.Where(s => s.Name == name).FirstOrDefault();
            return stadium;
        }

        
        //PUT
        public bool UpdateStadium(Stadium stadium)
        {
            _context.Update(stadium);
            return Save();
        }

        //POST
        public bool CreateStadium(Stadium stadium)
        {
            _context.Add(stadium);
            return Save();
        }

        //DELTE
        public bool DeleteStadium(Stadium stadium)
        {
            _context.Remove(stadium);
            return Save();
        }

        //SAVE
         public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
