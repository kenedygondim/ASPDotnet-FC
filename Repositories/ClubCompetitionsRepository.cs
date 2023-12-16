using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using ASPDotnetFC.Data.Context;

namespace ASPDotnetFC.Repository
{
    public class ClubCompetitionsRepository : IClubCompetitionsRepository
    {
        private readonly FootballContext _context;

        public ClubCompetitionsRepository(FootballContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }


        public bool AssociateClubCompetition(Competition competition, Club club)
        {
            var clubCompetition = new ClubCompetition
            {
                Competition = competition,
                Club = club
            };

            _context.Add(clubCompetition);

            return Save();
        }

        public ClubCompetition GetOne(int clubId, int competitionId )
        {
            return _context.ClubCompetitions.Where(cl => cl.ClubId == clubId && cl.CompetitionId ==  competitionId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

    }
}
