using ASPDotnetFC.Dto;
using ASPDotnetFC.Models;
using AutoMapper;

namespace ASPDotnetFC.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Club, ClubDto>();
            CreateMap<ClubDto, Club>();

            CreateMap<CompetitionDto, Competition>();
            CreateMap<Competition, CompetitionDto>();

            CreateMap<PlayerDto, Player>();
            CreateMap<Player, PlayerDto>();

            CreateMap<StadiumDto, Stadium>();
            CreateMap<Stadium, StadiumDto>();
        }
    }
}
