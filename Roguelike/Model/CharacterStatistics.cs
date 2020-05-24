using System;

namespace Roguelike.Model
{
    /// <summary>
    /// An information about character.
    /// </summary>
    public class CharacterStatistics : ICloneable
    {
        /// <summary>
        /// Damage to other characters.
        /// </summary>
        public int Force { get; set; }
        
        /// <summary>
        /// Health points, when it comes to 0, the character dies.
        /// </summary>
        public int Health { get; set; }
        
        /// <summary>
        /// Just for statistics.
        /// </summary>
        public int Experience { get; set; }

        public CharacterStatistics(int force, int health, int experience)
        {
            Force = force;
            Health = health;
            Experience = experience;
        }
        
        public object Clone() => MemberwiseClone();
    }
}
