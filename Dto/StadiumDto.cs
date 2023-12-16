using ASPDotnetFC.Models;
using System.Text.Json.Serialization;

namespace ASPDotnetFC.Dto
{
    public class StadiumDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Capacity { get; set; }
        public required string ConstructionYear { get; set; }
    }
}
