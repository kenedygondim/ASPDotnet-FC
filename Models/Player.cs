using System.Globalization;
using System.Text.Json.Serialization;

namespace ASPDotnetFC.Models
{
    public class Player
    {
        public int Id {  get; set; }
        public required string Fullname { get; set; }
        public required string Nickname { get; set; }
        public string? UrlImage { get; set; }
        public required int TeamGoals { get; set; }
        public required int TeamMatches { get; set; }
        public required string Birth { get; set; }
        public required string Position { get; set; }
        public required string Country { get; set; }
        public double? Heigth { get; set; }
        public string? BestFoot { get; set;}

        [JsonIgnore]
        public int?  ClubId { get; set; }
        [JsonIgnore]
        public virtual Club? Club { get; set; }
    }
}
