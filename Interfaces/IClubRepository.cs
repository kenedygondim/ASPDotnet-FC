using ASPDotnetFC.Models;

namespace ASPDotnetFC.Interface
{
    public interface IClubRepository 
    {
        ICollection<Club> GetClubs();

        Club GetClubById (int id);

        ICollection<Competition> GetCompetitions(Club club);

        Club GetClubByName (string name);

        int GetFoundationYear(int Id);

        string GetCountryName (int Id);

        Stadium GetStadium(int Id);

        bool CreateClub(int competitionId, Club club);

        bool UpdateClub(Club club);
            
        bool DeleteClub(Club club);

        bool Save();
    }
}
