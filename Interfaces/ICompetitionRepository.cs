using ASPDotnetFC.Models;

namespace ASPDotnetFC.Interface
{
    public interface ICompetitionRepository
    {
        ICollection<Competition> GetCompetitions();

        Competition GetCompetition(int id);

        int GetNumberOfTeams(int id);

        string GetCountryCompetition(int id);

        bool CreateCompetition(Competition competition);

        bool UpdateCompetition(Competition competition);

        bool DeleteCompetition(Competition competition);

        bool Save();

    }
}