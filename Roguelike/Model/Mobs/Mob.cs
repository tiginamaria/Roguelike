using System;
using System.Threading;
using System.Threading.Tasks;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.Mobs
{
    public class Mob : Character
    {
        private static int lastId;
        
        private readonly Level level;
        private readonly IMobBehaviour originalBehaviour;
        private readonly CharacterStatistics statistics;
        private const int ConfusionTimeMs = 5000;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private bool cancelled = true;

        public readonly int Id;
        protected IMobBehaviour Behaviour { get; set; }


        public event EventHandler OnDie;

        public Mob(Level level, IMobBehaviour behaviour, Position startPosition, 
            CharacterStatistics statistics, bool confused = false) : base(startPosition)
        {
            Id = lastId + 1;
            lastId++;
            
            this.level = level;
            Behaviour = behaviour;
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
            statistics.Health = Math.Max(0, statistics.Health - 1);
            if (statistics.Health == 0)
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

        protected virtual void BecomeConfused()
        {
            Task.Delay(ConfusionTimeMs, cancellation.Token).ContinueWith(t =>
            {
                if (cancelled)
                {
                    return;
                }
                Behaviour = originalBehaviour;
                cancelled = true;
            });

            Behaviour = new ConfusedMobBehaviour();
            cancelled = false;
        }

        public Position GetMove()
        {
            return Behaviour.MakeMove(level, Position);
        }

        public override string GetStringType()
        {
            return Behaviour.GetStringType();
        }
    }
}
