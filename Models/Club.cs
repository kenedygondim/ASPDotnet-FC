
namespace ASPDotnetFC.Models
{
    public class Club
    {

        public int Id { get; set; }
        public required string FullName { get; set; } 
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? UrlImage { get; set; }
        public required int FoundationYear { get; set; }
        public string? Mascot { get; set; } 
        public string? OfficialSite { get; set; }
        public required string Country { get; set; } 
        public virtual Player? TopScorer { get; set; }
        public virtual Stadium? Stadium { get; set; } 
        public virtual ICollection<ClubCompetition>? ClubCompetitions { get; set; }
    }
}
