using Epsic.Rpg.Models;
using Epsic.Rpg.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epsic.Rpg.Controllers
{
    [ApiController]
    public class CharactersController : ControllerBase
    {
        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        
        private readonly ICharacterService _characterService;

        [HttpGet("characters")]
        public IActionResult GetAll()
        {
            return Ok(_characterService.GetAll());
        }

        [HttpGet("characters/{id}")]
        public IActionResult GetSingle(int id)
        {
            if (id < 1) return BadRequest();
            var c = _characterService.GetSingle(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost("characters/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CharacterPatchViewModel model)
        {
            if (id < 1) return BadRequest();
            if (model.Name.Length > 32) return BadRequest();
            if (_characterService.ExistsByName(model.Name)) return BadRequest();
            if (!_characterService.ExistsById(id)) return NotFound();

            return Ok(_characterService.Update(id, model));
        }

        [HttpPost("characters")]
        public IActionResult AddCharacter(Character newCharacter)
        {
            if (newCharacter.Id < 1) return BadRequest();
            if (newCharacter.Name.Length > 32) return BadRequest();
            if (_characterService.ExistsByName(newCharacter.Name)) return BadRequest();
            if (_characterService.ExistsById(newCharacter.Id)) return BadRequest();

            _characterService.AddCharacter(newCharacter);

            return Created($"characters/{newCharacter.Id}", newCharacter);
        }

        [HttpGet("personnages")]
        public IActionResult Search(string name)
        {
            if (name == null) return Unauthorized();
            if (name.Length < 4) return Unauthorized();
            if (name == "teapot") return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status418ImATeapot);
            return Ok(_characterService.Search(name));
        }

        [HttpDelete("characters/{id}")]
        public IActionResult Delete(int id)
        {
            try {
                _characterService.Delete(id);
                return NoContent();
            } catch {
                return NoContent();
            }
        }
    }
}
