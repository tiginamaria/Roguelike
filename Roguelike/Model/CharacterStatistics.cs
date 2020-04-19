namespace Roguelike.Model
{
    public class CharacterStatistics
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
    }
}