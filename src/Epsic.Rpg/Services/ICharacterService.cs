using System.Collections.Generic;
using Epsic.Rpg.Models;

namespace Epsic.Rpg.Services
{
    public interface ICharacterService
    {
        Character AddCharacter(Character newCharacter);
        void Delete(int id);
        bool ExistsById(int id);
        bool ExistsByName(string name);
        IList<Character> GetAll();
        Character GetSingle(int id);
        IList<Character> Search(string name);
        Character Update(int id, CharacterPatchViewModel model);
    }
}