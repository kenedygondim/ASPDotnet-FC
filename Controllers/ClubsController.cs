using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASPDotnetFC.Dto;

namespace ASPDotnetFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubRepository _clubRepository;

        //nesse caso o auto mapper vai ajudar a selecionar as informações que queremos que o usuário veja
        //arquivos Dto's e MappingProfiles
        private readonly IMapper _mapper;

        public ClubsController(IClubRepository clubRepository, IMapper mapper)
        {
            _clubRepository = clubRepository;
            _mapper = mapper;

        }

        //!!!!!!!!!!!!!!!!!!!!! GET !!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Retorna todos os clubes do banco de dados
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Club>))]
        public IActionResult GetClubs()
        {
            var clubs = _mapper.Map<List<ClubDto>>(_clubRepository.GetClubs());

            if (!ModelState.IsValid)
            {
                return NotFound(ModelState);
            }


            return Ok(clubs);
        }

        //Retorna um clubes específico com base no Id
        [HttpGet("{clubId}")]
        [ProducesResponseType(200, Type = typeof(Club))]
        [ProducesResponseType(400)]
        public IActionResult GetClubById(int clubId)
        {
            var club = _mapper.Map<ClubDto>(_clubRepository.GetClubById(clubId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (club == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(club);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{ClubId}/competitions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCompetitionsByClub([FromRoute] int ClubId)
        {
            var club = _clubRepository.GetClubById(ClubId);

            if (club == null)
                return NotFound(ModelState);

            if (!ModelState.IsValid)  
                return BadRequest(ModelState);

            var competitions = _clubRepository.GetCompetitions(club);

            return Ok(competitions);
        }




        //Retorna um clubes específico com base no nome
        [HttpGet("{clubname}/name")]
        [ProducesResponseType(200, Type = typeof(Club))]
        [ProducesResponseType(400)]

        public IActionResult GetClubByName([FromRoute] string clubname)
        {

            //deixa a primeira letra da string maiúscula
            string stringFormatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(clubname);

            var club = _mapper.Map<ClubDto>(_clubRepository.GetClubByName(stringFormatted));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (club == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(club);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("{clubId}/clubCountry")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetClubCountry(int clubId)
        {
            var country = _clubRepository.GetCountryName(clubId);

            if (!ModelState.IsValid)
                return BadRequest();


            if (country == null)
                return NotFound();

            try { return Ok(country); }
            catch (Exception ex) { return BadRequest(ex.Message); };
        }

        [HttpGet("{clubId}/clubfoundationyear")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult GetFoundationYear(int clubId)
        {
            var foundationYear = _clubRepository.GetFoundationYear(clubId);

            if (!ModelState.IsValid)
                return BadRequest();

            try { return Ok(foundationYear); }
            catch (Exception ex) { return BadRequest(ex.Message); };
        }



        [HttpGet("{clubId}/clubstadium")]
        [ProducesResponseType(200, Type = typeof(Stadium))]
        [ProducesResponseType(400)]
        public IActionResult GetClubStadium(int clubId)
        {
            var stadium = _clubRepository.GetStadium(clubId);

            if (!ModelState.IsValid)
                return BadRequest();


            if (stadium == null)
                return NotFound();

            try { return Ok(stadium); }
            catch (Exception ex) { return BadRequest(ex.Message); };
        }


        [HttpPost("{competitionId}/addclub")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateClub([FromRoute] int competitionId, [FromBody] ClubDto clubCreate)

        {

            if (clubCreate == null)
                return BadRequest(ModelState);

            //recebe todas as ligas
            var clubs = _clubRepository.GetClubs();

            //verifica se a liga já existe:
            var checkClubExists = clubs.Where(
                l => l.Name.Trim().ToUpper() == clubCreate.Name.TrimEnd().ToUpper()
                ).FirstOrDefault();

            //Se a liga já existir, o erro é lançado
            if (checkClubExists != null)
            {
                ModelState.AddModelError("", "Esse time já existe!");
                return StatusCode(422, ModelState);
            }

            //se os dados inseridos não estiverem no padrão correto é lançado um 400
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //ocorre o mapeamento/'conversão' do clube criado para o modelo original de Club
            var clubMap = _mapper.Map<Club>(clubCreate);

            //chamada da função dentro do if
            if (!_clubRepository.CreateClub(competitionId, clubMap))
            {
                ModelState.AddModelError("", "Algo deu errado durante o salvamento =(");
                return StatusCode(500, ModelState);
            }


            try
            {
                //esse método retorna um código de criação 204
                //parâmetros:
                //nome do método    //parâmetro de rota do controllador  //Clube criado
                return CreatedAtAction(nameof(GetClubById), new { clubId = clubCreate.Id }, clubCreate);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };
        }


        [HttpPut("{clubId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClub(int clubId, [FromBody] ClubDto updatedClub)
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


        [HttpDelete("{clubId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteClub(int clubId)
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





    }
}
