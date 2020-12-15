using System;
using System.Collections.Generic;

namespace Epsic.Rpg.Models
{
    public class Team
    {
        public Team()
        {
            Characters = new List<Character>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Character> Characters { get; set; }
    }

    public class TeamSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TeamDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CharacterSummaryViewModel> Characters { get; set; }
    }

    public class UpdateTeamDto 
    {
        public string Name { get; set; }
    }

    public class CreateTeamDto 
    {
        public string Name { get; set; }
    }

    public class AddCharacterToTeamDto 
    {
        public int TeamId { get; set; }
        public int CharacterId { get; set; }
    }

    public class RemoveCharacterFromTeamDto 
    {
        public int TeamId { get; set; }
        public int CharacterId { get; set; }
    }
}