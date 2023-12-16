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
        public string GetCountryCompetition(int id)
        {
            var competition = _context.Competitions.Where(c => c.Id == id).FirstOrDefault();

            if (competition == null)
            {
                return "Não encontrado.";
            }

            return competition.Country;
        }

        public Competition GetCompetition(int id)
        {
            return _context.Competitions.Where(l => l.Id == id).FirstOrDefault();
        }

        public ICollection<Competition> GetCompetitions()
        {
            return _context.Competitions.Include(c => c.ClubCompetitions).ThenInclude(cl => cl.Club).ToList();
        }

        public int GetNumberOfTeams(int id)

        {
            var competition = _context.Competitions.Where(l => l.Id == id).FirstOrDefault();

            if (competition == null)
            {
                return 0;
            }

            return competition.NumberOfTeams;
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
