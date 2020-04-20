namespace Roguelike.Model
{
    public class Mob : GameObject, ICharacter
    {
        private readonly Level level;
        private readonly IMobBehaviour behaviour;
        private readonly CharacterStatistics statistics = new CharacterStatistics(1, 2, 1);

        public Mob(Level level, IMobBehaviour behaviour, Position startPosition) : base(startPosition)
        {
            this.level = level;
            this.behaviour = behaviour;
        }

        public CharacterStatistics GetStatistics()
        {
            return statistics;
        }

        public void Confuse(ICharacter other)
        {
            other.AcceptConfuse(this);
            statistics.Experience++;
            statistics.Force += other.GetStatistics().Force / 2;
        }

        public void AcceptConfuse(ICharacter other)
        {
            statistics.Health--;
            if (statistics.Health == 0)
            {
                level.Board.SetObject(Position, new EmptyCell(Position));
                Position = new Position(-10, -10);
            }
        }

        public void MakeMove()
        {
            Position = behaviour.MakeMove(level, Position);
        }
    }
}