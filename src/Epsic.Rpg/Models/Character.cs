using Epsic.Rpg.Enums;

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
    }

    public class CharacterPatchViewModel
    {
        public string Name { get; set; }
        public RpgClass Class { get; set; }
    }
}