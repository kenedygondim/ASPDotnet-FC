using ASPDotnetFC.Models;

namespace ASPDotnetFC.Interface
{
    public interface IClubCompetitionsRepository
    {
        ClubCompetition GetOne(int clubId, int competitionId);

        bool AssociateClubCompetition(Competition competition, Club club);

        bool Save();
    }
}
