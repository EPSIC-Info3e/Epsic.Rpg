using Epsic.Rpg.Enums;
using Microsoft.AspNetCore.Http;

namespace Epsic.Rpg.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Intelligence { get; set; }
        public RpgClass Class { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public byte[] Avatar { get; set; }
    }

    public class CharacterSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CharacterPatchViewModel
    {
        public string Name { get; set; }
        public RpgClass Class { get; set; }
    }
}