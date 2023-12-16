using System.Text.Json.Serialization;

namespace ASPDotnetFC.Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Capacity { get; set; }
        public required string ConstructionYear { get; set; }
        [JsonIgnore]
        public virtual ICollection<Club>? HomeClubs { get; set; }
    }
}
