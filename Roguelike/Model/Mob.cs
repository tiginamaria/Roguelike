using System;

namespace Roguelike.Model
{
    public class Mob : Character
    {
        private readonly Level level;
        private readonly IMobBehaviour behaviour;
        private readonly CharacterStatistics statistics = new CharacterStatistics(2, 2, 1);

        public event EventHandler OnDie;

        public IMobBehaviour Behaviour => behaviour;

        public Mob(Level level, IMobBehaviour behaviour, Position startPosition) : base(startPosition)
        {
            this.level = level;
            this.behaviour = behaviour;
        }

        public override CharacterStatistics GetStatistics()
        {
            return statistics;
        }

        public override void Confuse(Character other)
        {
            other.AcceptConfuse(this);
            statistics.Experience++;
            statistics.Force += other.GetStatistics().Force / 2;
        }

        public override void AcceptConfuse(Character other)
        {
            statistics.Health--;
            if (statistics.Health == 0)
            {
                level.Board.DeleteObject(Position);
                OnDie?.Invoke(this, EventArgs.Empty);
            }
        }

        public Position GetMove()
        {
            return behaviour.MakeMove(level, Position);
        }
    }
}