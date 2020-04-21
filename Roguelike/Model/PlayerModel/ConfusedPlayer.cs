using System;
using System.Threading;
using System.Threading.Tasks;
using Roguelike.Model.Objects;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// Represents a confused player.
    /// Every move may be reflected with the 0.5 probability.
    /// </summary>
    public class ConfusedPlayer : AbstractPlayer
    {
        private const int ConfusionTimeMs = 6000;
        private readonly Level level;
        private readonly AbstractPlayer player;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private static readonly Random Random = new Random();

        public ConfusedPlayer(Level level, AbstractPlayer player) : base(player.Position)
        {
            this.level = level;
            this.player = player;
            Task.Delay(ConfusionTimeMs, cancellation.Token).ContinueWith(t => 
                {
                    if (!cancellation.IsCancellationRequested)
                    {
                        level.Player = this.player;
                    } 
                });
        }

        private static int Confusion()
        {
            return Random.Next(2) == 1 ? 1 : -1;
        }

        public override void Move(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            player.Move(Confusion() * intentVerticalMove, Confusion() * intentHorizontalMove, board);
            Position = player.Position;
        }

        public override CharacterStatistics GetStatistics()
        {
            return player.GetStatistics();
        }

        public override void Confuse(Character other)
        {
            player.Confuse(other);
        }

        public override void AcceptConfuse(Character other)
        {
            cancellation.Cancel();
            level.Player = player;
            player.AcceptConfuse(other);
        }
    }
}
