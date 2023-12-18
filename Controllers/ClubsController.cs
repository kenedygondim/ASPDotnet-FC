using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASPDotnetFC.Dto;
using aspdotnetfc_api.Repositories;
using aspdotnetfc_api.Interfaces;
using System.Diagnostics.Metrics;

namespace ASPDotnetFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubRepository _clubRepository;
        private readonly IStadiumRepository _stadiumRepository;
        private readonly IMapper _mapper;

        public ClubsController(IClubRepository clubRepository, IMapper mapper, IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
            _clubRepository = clubRepository;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ClubDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetClubs()
        {
            try
            {
                var clubs = _mapper.Map<List<ClubDto>>(_clubRepository.GetClubs());

                if (!ModelState.IsValid)
                    return NotFound(ModelState);

                return Ok(clubs);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("{clubId}")]
        [ProducesResponseType(200, Type = typeof(ClubDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetClubById([FromRoute] int clubId)
        {
            try 
            {
                var club = _mapper.Map<ClubDto>(_clubRepository.GetClubById(clubId));

                if (club == null)
                    return NotFound("Clube não encontrado!");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(club);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{clubName}/name")]
        [ProducesResponseType(200, Type = typeof(ClubDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult GetClubByName([FromRoute] string clubname)
        {
            try
            {
                string stringFormatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(clubname);

                var club = _mapper.Map<ClubDto>(_clubRepository.GetClubByName(stringFormatted));

                if (club == null)
                    return NotFound("Clube não encontrado!");

                if (!ModelState.IsValid)      
                    return BadRequest(ModelState);            
            
                return Ok(club);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("{clubId}/clubCompetitions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CompetitionDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCompetitionsByClub([FromRoute] int clubId)
        {
            try
            {
                var club = _clubRepository.GetClubById(clubId);

                if (club == null)
                    return NotFound(ModelState);

                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);

                var competitions = _clubRepository.GetCompetitions(club);

                return Ok(competitions);
            }

            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("{clubId}/clubStadium")]
        [ProducesResponseType(200, Type = typeof(Stadium))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetClubStadium([FromRoute] int clubId)
        {
            try 
            {
                var clubStadium = _clubRepository.GetClubById(clubId);

                if (clubStadium == null)
                    return NotFound("Estadio não encontrado!");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(clubStadium.Stadium); 
            }

            catch (Exception ex) 
            {
                return BadRequest(ex.Message); 
            }
        }


        [HttpPost("{competitionId}/addClub")]
        [ProducesResponseType(204, Type = typeof(ClubDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public IActionResult CreateClubWithCompetition([FromRoute] int competitionId, [FromBody] ClubDto clubCreate)
        {
            try
            {
                if (clubCreate == null)
                return BadRequest(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var clubs = _clubRepository.GetClubs();
                var checkClubExists = clubs.Where(club => club.Name.Trim().ToUpper() == clubCreate.Name.TrimEnd().ToUpper())
                    .FirstOrDefault();

                if (checkClubExists != null)
                    return Conflict("Esse clube já existe!");

                var clubMap = _mapper.Map<Club>(clubCreate);

                if (!_clubRepository.CreateClubWithCompetition(competitionId, clubMap))
                {
                    ModelState.AddModelError("", "Algo deu errado durante o salvamento =(");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtAction(nameof(GetClubById), new { clubId = clubCreate.Id }, clubCreate);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost]
        [ProducesResponseType(204, Type = typeof(ClubDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public IActionResult CreateClub([FromBody] Club clubCreate)
        {
            try
            {
                if (clubCreate == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                var clubs = _clubRepository.GetClubs();

                var checkclub = clubs.Where(
                    l => l.Name.Trim().ToUpper() == clubCreate.Name.TrimEnd().ToUpper()).
                    FirstOrDefault();

                if (checkclub != null)
                    return Conflict("Esse clube já existe!");


                var clubMap = _mapper.Map<Club>(clubCreate);

                if (!_clubRepository.CreateClub(clubMap))
                {
                    ModelState.AddModelError("", "Algo deu errado durante o salvamento =(");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtAction(nameof(GetClubById), new { clubId = clubCreate.Id }, clubCreate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("club/{clubId}/stadium/{stadiumId}/associate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult AssociateClubStadium([FromRoute] int stadiumId, [FromRoute] int clubId)
        {
            try
            {
                var club = _clubRepository.GetClubById(clubId);
                var stadium = _stadiumRepository.GetStadiumById(stadiumId);

                if (club == null || stadium == null ) 
                    return NotFound(ModelState);

                if(club.Stadium != null)
                    return BadRequest("Já existe um estádio associado a essa clube");

                if (!ModelState.IsValid)            
                    return BadRequest(ModelState);


                if(!_clubRepository.AssociateClubStadium(club, stadium))
                {
                    ModelState.AddModelError("", "Algo deu errado durante a alteração.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Estádio adicionado ao clube com sucesso");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPut("{clubId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClub(int clubId, [FromBody] ClubDto updatedClub)
        {
            try
            {
                if (clubId != updatedClub.Id)
                    return NotFound(ModelState);

                if (updatedClub == null)
                    return BadRequest(ModelState);
            
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var clubMap = _mapper.Map<Club>(updatedClub);

                if (!_clubRepository.UpdateClub(clubMap))
                {
                    ModelState.AddModelError("", "Ocorreu um erro na sua tentativa de atualizar o clube!");
                    return StatusCode(500, ModelState);
                }

                return Ok("Clube atualizado com sucesso!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{clubId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteClub(int clubId)
        {
            try
            {
                var club = _clubRepository.GetClubById(clubId);

                if(club == null)
                    return NotFound(ModelState);
            

                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);

                if (!_clubRepository.DeleteClub(club))
                {
                    ModelState.AddModelError("", "Algo deu errado durante a exclusão.");
                    return StatusCode(500, ModelState); 
                }

                return Ok("Clube excluído");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
    }
}
