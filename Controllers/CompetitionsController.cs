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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Competition>))]
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

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Club))]
        public IActionResult GetCompetition(int id)
        {
            var competition = _mapper.Map<CompetitionDto>(_competitionRepository.GetCompetition(id));

            if(competition == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(competition);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{clubId}/countryCompetition")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryCompetition(int clubId)
        {
            var country = _competitionRepository.GetCountryCompetition(clubId);

            if (!ModelState.IsValid)
                return BadRequest();


            if (country == null)
                return NotFound();

            try { return Ok(country); }
            catch (Exception ex) { return BadRequest(ex.Message); };
        }




        [HttpGet("{competitionId}/numberOfTeams")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult GetNumberOfTeams(int competitionId)
        {
            var numberOfTeams = _competitionRepository.GetNumberOfTeams(competitionId);

            if (!ModelState.IsValid)
                return BadRequest();

            try { return Ok(numberOfTeams); }
            catch (Exception ex) { return BadRequest(ex.Message); };
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCompetition([FromBody] Competition competitionCreate)
        {
            if (competitionCreate == null)
                return BadRequest();

            //recebe todas as ligas
            var competitions = _competitionRepository.GetCompetitions();

            //verifica se a liga já existe:
            var checkCompetition = competitions.Where(
                l => l.CompetitionName.Trim().ToUpper() == competitionCreate.CompetitionName.TrimEnd().ToUpper()
                ).FirstOrDefault();

            //Se a liga já existir, o erro é lançado
            if(checkCompetition != null)
            {
                ModelState.AddModelError("", "Essa competição já existe!");
                return StatusCode(422, ModelState);
            }

            //se os dados inseridos não estiverem no padrão correto é lançado um 400
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
                
            _competitionRepository.CreateCompetition(competitionCreate);

            return Ok(competitionCreate); 
        }

        //PUT
        [HttpPut("{competitionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCompetition(int competitionId, [FromBody] CompetitionDto updatedCompetition)
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

        [HttpDelete("{competitionId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteCompetition(int competitionId)
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


    } }