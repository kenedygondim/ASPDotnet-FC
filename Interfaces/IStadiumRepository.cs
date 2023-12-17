using ASPDotnetFC.Models;

namespace aspdotnetfc_api.Interfaces
{
    public interface IStadiumRepository
    {
        ICollection<Stadium> GetStadiums();

        Stadium GetStadiumById(int id);

        Stadium GetStadiumByName(string name);

        bool CreateStadium(Stadium stadium);

        bool UpdateStadium(Stadium stadium);

        bool DeleteStadium(Stadium stadium);

        bool Save();
    }
}
