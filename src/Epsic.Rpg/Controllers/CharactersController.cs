using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epsic.Rpg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IActionResult Get()
        {
            return Ok(_characters.FirstOrDefault());
        }

        [HttpGet("characters/getsingle/{id}")]
        public IActionResult GetSingle(int id)
        {
            return Ok(_characters.FirstOrDefault(c => c.Id == id));
        }

        [HttpGet("characters/getall")]
        public IActionResult GetAll()
        {
            return Ok(_characters);
        }

        [HttpGet("personnages")]
        public IActionResult Search(string nom)
        {
            return Ok(_characters.Where(c => c.Name.Contains(nom)).ToList());
        }
    }
}
