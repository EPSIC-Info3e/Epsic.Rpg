using System.Collections.Generic;
using System.Linq;
using Epsic.Rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace Epsic.Rpg.Controllers
{
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private IList<Character> _characters = new List<Character> 
        {
            new Character { Id = 1, Name = "Pierre"},
            new Character { Id = 2, Name = "Paul"},
            new Character { Id = 3, Name = "Jacques"},
        };

        [HttpGet("characters")]
        public IActionResult GetAll()
        {
            return Ok(_characters);
        }

        [HttpGet("characters/{id}")]
        public IActionResult GetSingle(int id)
        {
            return Ok(_characters.FirstOrDefault(c => c.Id == id));
        }

        [HttpPost("characters/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CharacterPatchViewModel model)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);

            character.Name = model.Name;
            character.Class = character.Class;

            return Ok(character);
        }

        [HttpPost("characters")]
        public IActionResult AddCharacter(Character newCharacter)
        {
            _characters.Add(newCharacter);
            return Ok(newCharacter);
        }

        [HttpGet("personnages")]
        public IActionResult Search(string name)
        {
            return Ok(_characters.Where(c => c.Name.Contains(name)).ToList());
        }

        [HttpDelete("characters/{id}")]
        public IActionResult Delete(int id)
        {
            _characters.Remove(_characters.FirstOrDefault(c => c.Id == id));
            return Ok();
        }
    }
}
