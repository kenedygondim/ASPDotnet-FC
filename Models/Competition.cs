using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASPDotnetFC.Models
{
    public class Competition
    {

        public int Id { get; set; }
        public required string CompetitionName { get; set; } = string.Empty;
        public string? UrlImage { get; set; }
        public required int NumberOfTeams { get; set;}
        public required string Country { get; set; } = string.Empty;
        public required string FirstEdition { get; set; }
        public  bool? IsContinental { get; set; }
        public  bool? IsWorldwide {  get; set; }
        public virtual ICollection<ClubCompetition>? ClubCompetitions { get; set; }
         
    }
}
