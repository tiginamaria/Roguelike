using System;

namespace Roguelike.Model
{
    public static class CombatSystem
    {
        /// <summary>
        /// Set up statistics of characters after a fight.
        /// </summary>
        public static void Fight(Character attacker, Character victim, Level level)
        {
            var victimStatistics = victim.GetStatistics();
            var attackerStatistics = attacker.GetStatistics();

            victimStatistics.Health = Math.Max(0, victimStatistics.Health - attackerStatistics.Force / 2);
            victimStatistics.Experience = Math.Max(0, victimStatistics.Experience - 1);
            victim.BecomeConfused();
            if (victimStatistics.Health == 0)
            {
                victim.Delete(level.Board);
                victim.Die();
            }


            attackerStatistics.Experience++;
            attackerStatistics.Force += victimStatistics.Force / 2;
        }
    }
}