using System;
using System.Threading;
using System.Threading.Tasks;
using Roguelike.Model.Inventory;

namespace Roguelike.Model.Mobs
{
    /// <summary>
    /// A system controlled character.
    /// </summary>
    public class Mob : Character
    {
        private const int ConfusionTimeMs = 5000;
        
        private static int lastId;
        private readonly Level level;
        private readonly IMobBehaviour originalBehaviour;
        private readonly CharacterStatistics statistics;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private bool cancelled = true;

        /// <summary>
        /// A unique id.
        /// </summary>
        public readonly int Id;
        protected IMobBehaviour Behaviour { get; set; }

        /// <summary>
        /// An event that is risen when the mob dies.
        /// </summary>
        public event EventHandler OnDie;

        public Mob(Level level, IMobBehaviour behaviour, Position startPosition, 
            CharacterStatistics statistics, bool confused = false) : base(startPosition)
        {
            Id = ++lastId;
            
            this.level = level;
            Behaviour = behaviour;
            originalBehaviour = behaviour;
            this.statistics = statistics;

            if (confused)
            {
                BecomeConfused();
            }
        }
        public override CharacterStatistics GetStatistics() => statistics;

        public override void MakeDamage(Character other)
        {
            other.AcceptDamage(this);
            statistics.Experience++;
            statistics.Force += other.GetStatistics().Force / 2;
        }

        public override void Collect(InventoryItem inventory) => inventory.Apply(statistics);

        public override void AcceptDamage(Character other)
        {
            statistics.Health = Math.Max(0, statistics.Health - other.GetStatistics().Force / 2);
            if (statistics.Health == 0)
            {
                Delete(level.Board);
                OnDie?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (cancelled)
            {
                BecomeConfused();
            }
        }
        
        public IMobBehaviour GetBehaviour() => Behaviour;

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

        public Position GetMove() => Behaviour.MakeMove(level, Position);

        public override string GetStringType() => Behaviour.GetStringType();
    }
}
