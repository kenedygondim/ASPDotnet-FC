using ASPDotnetFC.Models;

namespace ASPDotnetFC.Dto
{
    public class CompetitionDto 
    {
        public int Id { get; set; }
        public required string CompetitionName { get; set; } = string.Empty;
        public string UrlImage { get; set; } = string.Empty;
        public required int NumberOfTeams { get; set; }
        public required string Country { get; set; } = string.Empty;
        public required string FirstEdition { get; set; }
        public required bool IsContinental { get; set; }
        public required bool IsWorldwide { get; set; }

    }
}
