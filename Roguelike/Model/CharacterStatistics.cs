using System;

namespace Roguelike.Model
{
    /// <summary>
    /// An information about character.
    /// </summary>
    public class CharacterStatistics : ICloneable
    {
        public int Force { get; set; }
        public int Health { get; set; }
        public int Experience { get; set; }

        public CharacterStatistics(int force, int health, int experience)
        {
            Force = force;
            Health = health;
            Experience = experience;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
