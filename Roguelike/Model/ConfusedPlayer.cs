using System;
using System.Threading;
using System.Threading.Tasks;

namespace Roguelike.Model
{
    public class ConfusedPlayer : AbstractPlayer
    {
        private const int ConfusionTimeMs = 10_000;
        private readonly AbstractPlayer player;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();

        public ConfusedPlayer(Level level, AbstractPlayer player) : base(player.Position)
        {
            this.player = player;
            Task.Delay(ConfusionTimeMs, cancellation.Token).ContinueWith(t => { level.Player = this.player; });
        }

        private static readonly Random Random = new Random();

        private static int Confusion()
        {
            return Random.Next(2) == 1 ? 1 : -1;
        }

        public override void Move(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            player.Move(Confusion() * intentVerticalMove, Confusion() * intentHorizontalMove, board);
        }

        public override CharacterStatistics GetStatistics()
        {
            return player.GetStatistics();
        }

        public override void Confuse(ICharacter other)
        {
            player.Confuse(other);
        }

        public override void AcceptConfuse(ICharacter other)
        {
            cancellation.Cancel();
            player.AcceptConfuse(other);
        }
    }
}