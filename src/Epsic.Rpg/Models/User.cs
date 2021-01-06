using System;
using System.Collections.Generic;

namespace Epsic.Rpg.Models
{
    public class User
    {
        public User()
        {
            Characters = new List<Character>();
        }
        
        public int Id { get; set; }
        public string Username { get; set; }
        public List<Character> Characters { get; set; }
    }

    public class UserSummaryViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class UserDetailViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<CharacterSummaryViewModel> Characters { get; set; }
    }

    public class CreateUserDto 
    {
        public string Username { get; set; }
    }

    public class UpdateUserDto 
    {
        public string Username { get; set; }
    }

    public class AddCharacterToUserDto 
    {
        public int UserId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveCharacterFromUserDto 
    {
        public int UserId { get; set; }
        public int CharacterId { get; set; }
    }
}