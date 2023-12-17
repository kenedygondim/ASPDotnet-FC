using ASPDotnetFC.Dto;
using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using ASPDotnetFC.Repository;
using aspdotnetfc_api.Interfaces;
using aspdotnetfc_api.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            var stadiums = _mapper.Map<List<StadiumDto>>(_stadiumRepository.GetStadiums());

            if(stadiums == null)
            {
                return NotFound(ModelState);
            }

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(stadiums);
        }

        [HttpGet("{stadiumId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetStadiumById([FromRoute] int stadiumId)
        {
            var stadium = _mapper.Map<StadiumDto>(_stadiumRepository.GetStadiumById(stadiumId));

            if (stadium == null)
            {
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(stadium);
        }

        [HttpGet("{stadiumName}/name")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetClubByName([FromRoute] string stadiumName)
        {

            //deixa a primeira letra da string maiúscula
            string stringFormatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stadiumName);

            var stadium = _mapper.Map<StadiumDto>(_stadiumRepository.GetStadiumByName(stringFormatted));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (stadium == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(stadium);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //POST

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCompetition([FromBody] Stadium stadiumCreate)
        {
            if (stadiumCreate == null)
                return BadRequest();


            var stadiums = _stadiumRepository.GetStadiums();

            var checkStadium = stadiums.Where(
                l => l.Name.Trim().ToUpper() == stadiumCreate.Name.TrimEnd().ToUpper()
                ).FirstOrDefault();

            if (checkStadium != null)
            {
                ModelState.AddModelError("", "Esse estádio já existe!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var stadiumMap = _mapper.Map<Stadium>(stadiumCreate);

            if (!_stadiumRepository.CreateStadium(stadiumMap))
            {
                ModelState.AddModelError("", "Algo deu errado durante o salvamento =(");
                return StatusCode(500, ModelState);
            }


            try
            {
                return Ok(stadiumMap);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };

        }


      


        [HttpPut("{stadiumId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStadium(int stadiumId, [FromBody] StadiumDto updatedStadium)
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


        [HttpDelete("{stadiumId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteStadium(int stadiumId)
        {
            var stadium = _stadiumRepository.GetStadiumById(stadiumId);

            if (stadium == null)
                return NotFound(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_stadiumRepository.DeleteStadium(stadium))
            {
                ModelState.AddModelError("", "Algo deu errado durante a exclusão.");
                return StatusCode(500, ModelState);
            }

            return Ok("Estádio excluído");
        }









    }
}
