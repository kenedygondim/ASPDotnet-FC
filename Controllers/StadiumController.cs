using ASPDotnetFC.Dto;
using ASPDotnetFC.Models;
using aspdotnetfc_api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnetfc_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private readonly IStadiumRepository _stadiumRepository;
        private readonly IMapper _mapper;

        public StadiumController(IStadiumRepository stadiumRepository, IMapper mapper)
        {
            _stadiumRepository = stadiumRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetStadiums()
        {
            try
            {
                var stadiums = _mapper.Map<List<StadiumDto>>(_stadiumRepository.GetStadiums());

                if(stadiums == null)
                    return NotFound("Nenhum estádio encontrado!");

                if(!ModelState.IsValid) 
                    return BadRequest(ModelState);

                return Ok(stadiums);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{stadiumId}")]
        [ProducesResponseType(200, Type = typeof(Stadium))]
        [ProducesResponseType(400)]
        public IActionResult GetStadiumById([FromRoute] int stadiumId)
        {
            try
            { 
                var stadium = _mapper.Map<StadiumDto>(_stadiumRepository.GetStadiumById(stadiumId));

                if (stadium == null)
                    return NotFound("Estádio não encontrado!");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(stadium);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("{stadiumName}/name")]
        [ProducesResponseType(200, Type = typeof(Stadium))]
        [ProducesResponseType(400)]

        public IActionResult GetClubByName([FromRoute] string stadiumName)
        {
            try
            {
                string stringFormatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stadiumName);
                var stadium = _mapper.Map<StadiumDto>(_stadiumRepository.GetStadiumByName(stringFormatted));

                if (stadium == null)
                    return NotFound("Estádio não encontrado!");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(stadium);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public IActionResult CreateCompetition([FromBody] Stadium stadiumCreate)
        {
            try
            {
                if (stadiumCreate == null)
                    return BadRequest(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var stadiums = _stadiumRepository.GetStadiums();
                var checkStadium = stadiums.Where(l => l.Name.Trim().ToUpper() == stadiumCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

                if (checkStadium != null)
                    return Conflict("Esse estádio já existe!");

                var stadiumMap = _mapper.Map<Stadium>(stadiumCreate);

                if (!_stadiumRepository.CreateStadium(stadiumMap))
                {
                    ModelState.AddModelError("", "Algo deu errado durante o salvamento =(");
                    return StatusCode(500, ModelState);
                }
                return Ok(stadiumMap);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPut("{stadiumId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStadium([FromRoute] int stadiumId, [FromBody] StadiumDto updatedStadium)
        {
            try
            {
                if (stadiumId != updatedStadium.Id)
                     return NotFound(ModelState);

                if (updatedStadium == null)
                    return BadRequest(ModelState);  

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var stadiumMap = _mapper.Map<Stadium>(updatedStadium);

                if (!_stadiumRepository.UpdateStadium(stadiumMap))
                {
                    ModelState.AddModelError("", "Ocorreu um erro na sua tentativa de atualizar o estádio!");
                    return StatusCode(500, ModelState);
                }

                return Ok("Estádio atualizado com sucesso!");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{stadiumId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        
        public IActionResult DeleteStadium([FromRoute] int stadiumId)
        {
            try
            {
                var stadium = _stadiumRepository.GetStadiumById(stadiumId);

                if (stadium == null)
                    return NotFound("Estádio não encontrado!");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_stadiumRepository.DeleteStadium(stadium))
                {
                    ModelState.AddModelError("", "Algo deu errado durante a exclusão.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Estádio excluído");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
