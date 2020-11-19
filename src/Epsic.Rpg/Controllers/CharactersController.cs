using System.Collections.Generic;
using System.Linq;
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
            return Ok(_characterService.GetSingle(id));
        }

        [HttpPost("characters/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CharacterPatchViewModel model)
        {
            return Ok(_characterService.Update(id, model));
        }

        [HttpPost("characters")]
        public IActionResult AddCharacter(Character newCharacter)
        {
            return Ok(_characterService.AddCharacter(newCharacter));
        }

        [HttpGet("personnages")]
        public IActionResult Search(string name)
        {
            return Ok(_characterService.Search(name));
        }

        [HttpDelete("characters/{id}")]
        public IActionResult Delete(int id)
        {
            _characterService.Delete(id);
            return Ok();
        }
    }
}
