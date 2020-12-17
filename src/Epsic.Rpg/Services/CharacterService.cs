using System;
using System.Collections.Generic;
using System.Linq;
using Epsic.Rpg.Data;
using Epsic.Rpg.Models;

namespace Epsic.Rpg.Services
{

    public class CharacterService : ICharacterService
    {
        private readonly EpsicRpgDataContext _context;
        
        public CharacterService(EpsicRpgDataContext context)
        {
            _context = context;
        }

        public IList<Character> GetAll()
        {
            return _context.Characters.ToList();
        }

        public Character GetSingle(int id)
        {
            return _context.Characters.FirstOrDefault(c => c.Id == id);
        }

        public Character Update(int id, CharacterPatchViewModel model)
        {
            var character = _context.Characters.FirstOrDefault(c => c.Id == id);

            character.Name = model.Name;
            character.Class = model.Class;

            _context.SaveChanges();

            return character;
        }

        public Character AddCharacter(Character newCharacter)
        {
            _context.Characters.Add(newCharacter);
            _context.SaveChanges();
            return newCharacter;
        }

        public IList<Character> Search(string name)
        {
            return _context.Characters.Where(c => c.Name.Contains(name)).ToList();
        }

        public void Delete(int id)
        {
            _context.Characters.Remove(_context.Characters.FirstOrDefault(c => c.Id == id));
            _context.SaveChanges();
        }

        public void SetAvatar(int id, byte[] image)
        {
            var caracter = _context.Characters.Find(id);
            caracter.Avatar = image;
            _context.SaveChanges();
        }

        public byte[] GetAvatar(int id)
        {
            return _context.Characters.Find(id).Avatar;
        }

        public bool ExistsById(int id)
        {
            return _context.Characters.Any(c => c.Id == id);
        }

        public bool ExistsByName(string name)
        {
            return _context.Characters.Any(c => c.Name.Contains(name));
        }
    }
}