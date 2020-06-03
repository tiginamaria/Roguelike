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

        public override void Fight(Character other) => CombatSystem.Fight(this, other, level);

        public override void Collect(InventoryItem inventory) => inventory.Apply(statistics);

        public IMobBehaviour GetBehaviour() => Behaviour;

        public override void BecomeConfused()
        {
            if (!cancelled) return;
            cancelled = false;
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
        }

        public Position GetMove() => Behaviour.MakeMove(level, Position);

        public override string GetStringType() => Behaviour.GetStringType();
    }
}