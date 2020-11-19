using System;
using System.Collections.Generic;
using System.Linq;
using Epsic.Rpg.Models;

namespace Epsic.Rpg.Services
{

    public class CharacterService : ICharacterService
    {
        private IList<Character> _characters = new List<Character>
        {
            new Character { Id = 1, Name = "Pierre"},
            new Character { Id = 2, Name = "Paul"},
            new Character { Id = 3, Name = "Jacques"},
        };

        public IList<Character> GetAll()
        {
            return _characters;
        }

        public Character GetSingle(int id)
        {
            return _characters.FirstOrDefault(c => c.Id == id);
        }

        public Character Update(int id, CharacterPatchViewModel model)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);

            character.Name = model.Name;
            character.Class = model.Class;

            return character;
        }

        public Character AddCharacter(Character newCharacter)
        {
            _characters.Add(newCharacter);
            return newCharacter;
        }

        public IList<Character> Search(string name)
        {
            return _characters.Where(c => c.Name.Contains(name)).ToList();
        }

        public void Delete(int id)
        {
            _characters.Remove(_characters.FirstOrDefault(c => c.Id == id));
        }
    }
}