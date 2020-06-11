using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Roguelike.Model.Inventory;
using Roguelike.Model.Objects;

namespace Roguelike.Model.PlayerModel
{
    /// <summary>
    /// Represents a confused player.
    /// Every move may be reflected with the 0.5 probability.
    /// </summary>
    public class ConfusedPlayer : AbstractPlayer
    {
        private const int ConfusionTimeMs = 5000;
        private readonly Level level;
        private readonly AbstractPlayer player;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private static readonly Random Random = new Random();
        private bool cancelled;

        public ConfusedPlayer(Level level, AbstractPlayer player) : base(player.Position, player.Login)
        {
            this.level = level;
            this.player = player;
            Task.Delay(ConfusionTimeMs, cancellation.Token).ContinueWith(t =>
            {
                if (!cancelled)
                {
                    RemoveEffect();
                }
            });
        }

        private static int GetConfusion() => Random.Next(2) == 1 ? 1 : -1;

        /// <summary>
        /// Randomly reflects the given vector.
        /// </summary>
        public static Position ConfuseIntent(Position intent) =>
            new Position(GetConfusion() * intent.Y, GetConfusion() * intent.X);

        /// <summary>
        /// Moves the player, randomly reflecting its intent.
        /// </summary>
        public override void Move(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            var newVerticalIntent = GetConfusion() * intentVerticalMove;
            var newHorizontalIntent = GetConfusion() * intentHorizontalMove;
            player.Move(newVerticalIntent, newHorizontalIntent, board);
            Position = player.Position;
        }

        /// <summary>
        /// Moves the player directly.
        /// </summary>
        public override void MoveStraightforward(int intentVerticalMove, int intentHorizontalMove, Board board)
        {
            player.MoveStraightforward(intentVerticalMove, intentHorizontalMove, board);
            Position = player.Position;
        }

        public override CharacterStatistics GetStatistics() => player.GetStatistics();

        public override void Fight(Character other) => player.Fight(other);

        public override void Collect(InventoryItem inventory) => player.Collect(inventory);

        /// <summary>
        /// Cancel confusion effect and apply it again.
        /// </summary>
        public override void BecomeConfused()
        {
            cancellation.Cancel();
            RemoveEffect();
            player.BecomeConfused();
        }

        private void RemoveEffect()
        {
            cancelled = true;
            level.UpdatePlayer(player);
            level.Board.SetObject(Position, player);
        }

        public override void PutOff(string inventoryType) => player.PutOff(inventoryType);

        public override void PutOn(string inventoryType) => player.PutOn(inventoryType);

        public override List<InventoryItem> GetInventory() => player.GetInventory();

        public override List<InventoryItem> GetAppliedInventory() => player.GetAppliedInventory();

        public override string GetStringType()
        {
            if (level.IsCurrentPlayer(this))
            {
                return PlayerType.ConfusedPlayer;
            }

            return PlayerType.EnemyConfusedPlayer;
        }

        public override void Die() => player.Die();
    }
}