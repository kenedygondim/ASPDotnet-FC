using ASPDotnetFC.Models;

namespace ASPDotnetFC.Interface
{
    public interface IClubRepository 
    {
        ICollection<Club> GetClubs();

        Club GetClubById (int id);

        ICollection<Competition> GetCompetitions(Club club);

        Club GetClubByName (string name);

        bool CreateClubWithCompetition(int competitionId, Club club);

        bool CreateClub(Club club);

        bool AssociateClubStadium(Club club, Stadium stadium);

        bool UpdateClub(Club club);

            
        bool DeleteClub(Club club);

        bool Save();
    }
}
