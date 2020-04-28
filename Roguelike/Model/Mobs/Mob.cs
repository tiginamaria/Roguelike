using System;
using System.Threading;
using System.Threading.Tasks;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.Mobs
{
    public class Mob : Character
    {
        private readonly Level level;
        private IMobBehaviour behaviour;
        private readonly IMobBehaviour originalBehaviour;
        private readonly CharacterStatistics statistics;
        
        private const int ConfusionTimeMs = 5000;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private bool cancelled = true;

        public event EventHandler OnDie;

        public IMobBehaviour Behaviour => behaviour;

        public Mob(Level level, IMobBehaviour behaviour, Position startPosition, CharacterStatistics statistics, bool confused = false) : base(startPosition)
        {
            this.level = level;
            this.behaviour = behaviour;
            originalBehaviour = behaviour;
            this.statistics = statistics;
            if (confused)
            {
                BecomeConfused();
            }
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

        public override void Collect(InventoryItem inventory)
        {
            inventory.Apply(statistics);
        }

        public override void AcceptConfuse(Character other)
        {
            statistics.Health--;
            if (statistics.Health <= 0)
            {
                level.Board.DeleteObject(Position);
                OnDie?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (cancelled)
            {
                BecomeConfused();
            }
        }

        private void BecomeConfused()
        {
            Task.Delay(ConfusionTimeMs, cancellation.Token).ContinueWith(t =>
            {
                if (cancelled) return;
                behaviour = originalBehaviour;
                cancelled = true;
            });

            behaviour = new ConfusedMobBehaviour();
            cancelled = false;
        }

        public Position GetMove()
        {
            return behaviour.MakeMove(level, Position);
        }

        public override string GetType()
        {
            return behaviour.GetType();
        }
    }
}
