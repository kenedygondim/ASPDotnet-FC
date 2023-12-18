using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using ASPDotnetFC.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPDotnetFC.Repository
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly FootballContext _context;

        public CompetitionRepository (FootballContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //GET METHODS

        public ICollection<Competition> GetCompetitions()
        {
            return _context.Competitions.Include(c => c.ClubCompetitions).ThenInclude(cl => cl.Club).ToList();
        }

        public Competition GetCompetition(int id)
        {
            return _context.Competitions.Where(l => l.Id == id).FirstOrDefault();
        }

        //POST METHODS
        public bool CreateCompetition(Competition competition)
        {
            _context.Add(competition); 
            return Save();
        }

        public bool UpdateCompetition(Competition competition)
        {
            _context.Update(competition);
            return Save();
        }

        public bool DeleteCompetition(Competition competition)
        {
            _context.Remove(competition);
            return Save();
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
