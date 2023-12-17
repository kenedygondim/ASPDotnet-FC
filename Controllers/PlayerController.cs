﻿using ASPDotnetFC.Dto;
using ASPDotnetFC.Interface;
using ASPDotnetFC.Models;
using aspdotnetfc_api.Interfaces;
using aspdotnetfc_api.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnetfc_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IMapper _mapper;

        public PlayerController(IPlayerRepository playerRepository, IMapper mapper, IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetPlayers()
        {
            var players = _mapper.Map<List<PlayerDto>>(_playerRepository.GetPlayers());

            if (players == null)
            {
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(players);
        }

        
            [HttpGet("{playerId}")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            public IActionResult GetPlayerById([FromRoute] int playerId)
            {
                var player = _mapper.Map<PlayerDto>(_playerRepository.GetPlayerById(playerId));

                if (player == null)
                {
                    return NotFound(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(player);
            }


            [HttpGet("{playerName}/name")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]

            public IActionResult GetPlayerByName([FromRoute] string playerName)
            {
                string stringFormatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(playerName);

                var player = _mapper.Map<PlayerDto>(_playerRepository.GetPlayerByName(stringFormatted));

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (player == null)
                {
                    return NotFound();
                }

                try
                {
                    return Ok(player);
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
            public IActionResult CreateCompetition([FromBody] Player playerCreate)
            {
                if (playerCreate == null)
                    return BadRequest();


                var players = _playerRepository.GetPlayers();

                var checkPlayer = players.Where(
                    l => l.Nickname.Trim().ToUpper() == playerCreate.Nickname.TrimEnd().ToUpper()
                    ).FirstOrDefault();

                if (checkPlayer != null)
                {
                    ModelState.AddModelError("", "Esse jogador já existe!");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var playerMap = _mapper.Map<Player>(playerCreate);

                if (!_playerRepository.CreatePlayer(playerMap))
                {
                    ModelState.AddModelError("", "Algo deu errado durante o salvamento =(");
                    return StatusCode(500, ModelState);
                }


                try
                {
                    return Ok(playerMap);
                }
                catch (Exception ex) { return BadRequest(ex.Message); };

            }


        [HttpPut("{playerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePlayer(int playerId, [FromBody] PlayerDto updatedPlayer)
        {

            if (playerId != updatedPlayer.Id)
                return NotFound(ModelState);

            if (updatedPlayer == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playerMap = _mapper.Map<Player>(updatedPlayer);

            if (!_playerRepository.UpdatePlayer(playerMap))
            {
                ModelState.AddModelError("", "Ocorreu um erro na sua tentativa de atualizar o playere!");
                return StatusCode(500, ModelState);
            }

            return Ok("Jogador atualizado com sucesso!");
        }


        [HttpDelete("{playerId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult Deleteplayer(int playerId)
        {
            var player = _playerRepository.GetPlayerById(playerId);

            if (player == null)
                return NotFound(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_playerRepository.DeletePlayer(player))
            {
                ModelState.AddModelError("", "Algo deu errado durante a exclusão.");
                return StatusCode(500, ModelState);
            }

            return Ok("Jogador excluído");
        }


        [HttpPut("club/{clubId}/player/{playerId}/associate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult AssociatePlayerClub([FromRoute] int playerId, [FromRoute] int clubId)
        {
            var club = _clubRepository.GetClubById(clubId);
            var player = _playerRepository.GetPlayerById(playerId);

            if (club == null || player == null)
            {
                return NotFound(ModelState);
            }

            if (club.TopScorer != null)
            {
                return BadRequest("Já existe um estádio associado a essa clube");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _playerRepository.AssociatePlayerClub(player, club);

            return Ok("Sucesso");
        }








    }
}