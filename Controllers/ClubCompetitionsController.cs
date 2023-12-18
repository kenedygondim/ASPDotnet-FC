using ASPDotnetFC.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotnetFC.Controllers
{
    [ApiController]
    public class ClubCompetitionsController : ControllerBase
    {
        private readonly IClubCompetitionsRepository _clubCompetitionRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IMapper _mapper;

        public ClubCompetitionsController(IClubCompetitionsRepository clubcompetitionRepository, IClubRepository clubRepository, ICompetitionRepository competitionRepository, IMapper mapper)
        {
            _clubCompetitionRepository = clubcompetitionRepository;
            _clubRepository = clubRepository;
            _competitionRepository = competitionRepository;
            _mapper = mapper;

        }

        //Esse método será responsável apenas por associar um clube EXISTENTE em uma liga EXISTENTE
        [HttpPost("/api/club/{clubId}/competition/{competitionId}/associate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public IActionResult AssociateClubCompetition([FromRoute ]int clubId, [FromRoute] int competitionId) 
        {
            try
            {
                var club = _clubRepository.GetClubById(clubId);
                var competition = _competitionRepository.GetCompetition(competitionId);
                var clubcompetitions = _clubCompetitionRepository.GetOne(clubId, competitionId);

                if (club == null)
                    return NotFound("Esse clube não existe.");

                if (competition == null)
                    return NotFound("Essa liga não existe.");

                if (clubcompetitions != null)
                    return Conflict("Esse clube já está associado a liga.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _clubCompetitionRepository.AssociateClubCompetition(competition, club);

                return Ok("Sucesso. Clube associado à competição!");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
