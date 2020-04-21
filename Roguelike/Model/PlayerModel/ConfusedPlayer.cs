using System;
using System.Threading;
using System.Threading.Tasks;
using Roguelike.Model.Objects;

namespace Roguelike.Model.PlayerModel
{
    public class ConfusedPlayer : AbstractPlayer
    {
        private const int ConfusionTimeMs = 10000;
        private readonly AbstractPlayer player;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private static readonly Random Random = new Random();

        public ConfusedPlayer(Level level, AbstractPlayer player) : base(player.Position)
        {
            this.player = player;
            Task.Delay(ConfusionTimeMs, cancellation.Token).ContinueWith(t => 
                { level.Player = this.player; });
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
            player.AcceptConfuse(other);
        }
    }
}
