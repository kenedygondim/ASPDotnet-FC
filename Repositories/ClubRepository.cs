using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using ASPDotnetFC.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ASPDotnetFC.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly FootballContext _context;

        public ClubRepository (FootballContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //GET methods
        public ICollection<Club> GetClubs()
        {
            return _context.Clubs.Include(c=>c.Stadium).Include(c => c.TopScorer).ToList();
        }
        
        public Club GetClubById(int id)
        {     
            return _context.Clubs.Where(c => c.Id == id).Include(c => c.Stadium).Include(c => c.TopScorer).Include(c=>c.ClubCompetitions).ThenInclude(c=> c.Competition).FirstOrDefault();
        }

        public ICollection<Competition> GetCompetitions (Club club)
        {     
            return club.ClubCompetitions.Select(c => c.Competition).ToList();
        }

        public Club GetClubByName(string name)
        {
            return _context.Clubs.Where(c => c.Name == name).FirstOrDefault();
        }

        public string GetCountryName(int id)
        {
            var club = _context.Clubs.Where(c => c.Id == id).FirstOrDefault();

            if(club == null)
            {
                return "Não encontrado.";
            }

            return club.Country;
        }

        public int GetFoundationYear(int id)
        {
            var club = _context.Clubs.Where(c => c.Id == id).FirstOrDefault();

            if (club == null)
            {
                return 0;
            }

            return club.FoundationYear;
        }

        public Stadium GetStadium(int id)
        {
            var club = _context.Clubs.Where(c => c.Id == id).FirstOrDefault();
            return club.Stadium;
        }

        //POST METHOD
        public bool CreateClub(int competitionId, Club club)
        {
            
            var clubCompetitionEntity = _context.Competitions.Find(competitionId);
            

            if (clubCompetitionEntity == null)
                return false;

            var clubCompetition = new ClubCompetition
            {
                Competition = clubCompetitionEntity,
                Club = club
            };


            _context.Add(club);
            _context.Add(clubCompetition);

            

            return Save();
        }

        
        //UPDATE method
        public bool UpdateClub(Club club)
        {
            _context.Update(club);
            return Save();
        }

        //DELETE method
        public bool DeleteClub(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        //Apenas salva alterações de GET, PUT E DELETE
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
            }
    }
}
