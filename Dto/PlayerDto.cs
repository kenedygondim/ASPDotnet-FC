using ASPDotnetFC.Models;
using System.Text.Json.Serialization;

namespace ASPDotnetFC.Dto
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public required string Fullname { get; set; }
        public required string Nickname { get; set; }
        public string? UrlImage { get; set; }
        public required int TeamGoals { get; set; }
        public required int TeamMatches { get; set; }
        public required string Birth { get; set; }
        public required string Position { get; set; }
        public required string Country { get; set; }
        public required double Heigth { get; set; }
        public required string BestFoot { get; set; }
    }
}
