using ASPDotnetFC.Models;

namespace ASPDotnetFC.Dto
{
    public class ClubDto

    {
        public int Id { get; set; }
        public required string FullName { get; set; } = string.Empty;
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? UrlImage { get; set; }
        public required int FoundationYear { get; set; }
        public required string Country { get; set; } = string.Empty;
        public virtual required Player? TopScorer { get; set; }
        public virtual required Stadium? Stadium { get; set; }

    }
}
