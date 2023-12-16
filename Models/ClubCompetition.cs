#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASPDotnetFC.Models
{
    public class ClubCompetition
    {

        public int ClubId { get; set; }

        public Club Club { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }

    }
}
