using ASPDotnetFC.Dto;
using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotnetFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IMapper _mapper;

        public CompetitionsController(ICompetitionRepository competitionRepository, IMapper mapper)
        {
            _competitionRepository = competitionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CompetitionDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCompetitions()
        {
            try
            {
                var competitions = _mapper.Map<List<CompetitionDto>>(_competitionRepository.GetCompetitions());

                return Ok(competitions);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{competitionId}")]
        [ProducesResponseType(200, Type = typeof(CompetitionDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCompetition([FromRoute] int competitionId)
        {
            try
            {
                var competition = _mapper.Map<CompetitionDto>(_competitionRepository.GetCompetition(competitionId));

                if (competition == null)
                    return NotFound("Competição não encontrada.");

                return Ok(competition);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CompetitionDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public IActionResult CreateCompetition([FromBody] Competition competitionCreate)
        {
            try
            {
                if (competitionCreate == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                var competitions = _competitionRepository.GetCompetitions();
                var checkCompetition = competitions.Where(
                    l => l.CompetitionName.Trim().ToUpper() == competitionCreate.CompetitionName.TrimEnd().ToUpper()
                    ).FirstOrDefault();

                if (checkCompetition != null)
                    return Conflict("Essa competição já existe!");

                var mapCompetition = _mapper.Map<Competition>(competitionCreate);

                if (!_competitionRepository.CreateCompetition(mapCompetition))
                {
                    ModelState.AddModelError("", "Um erro ocorreu!");
                    return StatusCode(500, ModelState);
                }

                return Ok(mapCompetition);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT
        [HttpPut("{competitionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCompetition(int competitionId, [FromBody] CompetitionDto updatedCompetition)
        {
            try
            {
                if (competitionId != updatedCompetition.Id)
                     return NotFound(ModelState);

                if (updatedCompetition == null)
                     return BadRequest(ModelState);

                if (!ModelState.IsValid)
                     return BadRequest(ModelState);

                var competitionMap = _mapper.Map<Competition>(updatedCompetition);

                if (!_competitionRepository.UpdateCompetition(competitionMap))
                {
                    ModelState.AddModelError("", "Ocorreu um erro na sua tentativa de atualizar a competição!");
                    return StatusCode(500, ModelState);
                }

                return Ok("Competição atualizada com sucesso!");
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }       
        }

        [HttpDelete("{competitionId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteCompetition(int competitionId)
        {
            try
            {
                var competition = _competitionRepository.GetCompetition(competitionId);

                if (competition == null)
                    return NotFound(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_competitionRepository.DeleteCompetition(competition))
                {
                    ModelState.AddModelError("", "Algo deu errado durante a exclusão.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Competição excluída");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    } 
}