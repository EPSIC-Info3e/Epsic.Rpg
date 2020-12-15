using System;
using System.Threading.Tasks;
using Epsic.Rpg.Exceptions;
using Epsic.Rpg.Models;
using Epsic.Rpg.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epsic.Rpg.Controllers
{
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsService _teamsService;
        private readonly ICharacterService _characterService;

        public TeamsController(ITeamsService teamsService, ICharacterService characterService)
        {
            _teamsService = teamsService;
            _characterService = characterService;
        }
        
        [HttpGet("teams")]
        public async Task<IActionResult> SearchAsync([FromQuery]string name)
        {
            try
            {
                return Ok(await _teamsService.GetAll(name));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("teams/{id}")]
        public async Task<IActionResult> GetSingleAsync(int id)
        {
            try
            {
                var result = await _teamsService.GetSingle(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("teams/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateTeamDto model)
        {
            try
            {
                return Ok(await _teamsService.UpdateAsync(id, model));
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("teams")]
        public async Task<IActionResult> CreateAsync(CreateTeamDto model)
        {
            try
            {
                var modelDb = await _teamsService.CreateAsync(model);

                return Created($"teams/{modelDb.Id}", modelDb);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _teamsService.Delete(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("teams/characters")]
        public async Task<IActionResult> RemoveCharacterFromTeamAsync(RemoveCharacterFromTeamDto removeCharacterFromTeam)
        {
            try
            {
                await _teamsService.RemoveCharacterFromTeam(removeCharacterFromTeam);
                return Ok();
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("teams/characters")]
        public async Task<IActionResult> AddCharacterToTeamAsync(AddCharacterToTeamDto addCharacterToTeam)
        {
            try
            {
                await _teamsService.AddCharacterToTeam(addCharacterToTeam);
                return Ok();
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
