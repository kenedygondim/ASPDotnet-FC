using ASPDotnetFC.Dto;
using ASPDotnetFC.Models;

namespace aspdotnetfc_api.Interfaces
{
    public interface IPlayerRepository
    {
        ICollection<Player> GetPlayers();
        
        Player GetPlayerById(int id);

        Player GetPlayerByName(string name);

        bool CreatePlayer(Player player);

        bool UpdatePlayer(Player player);

        bool AssociatePlayerClub(Player player, Club club);

        bool DeletePlayer(Player player);

        bool Save();

    }
}
