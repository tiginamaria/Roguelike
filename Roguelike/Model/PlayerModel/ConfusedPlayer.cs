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

        public ConfusedPlayer(Level level, AbstractPlayer player) : base(player.Position)
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

        public override void Collect(InventoryItem inventory)
        {
            player.Collect(inventory);
        }

        public override void AcceptConfuse(Character other)
        {
            cancellation.Cancel();
            RemoveEffect();
            player.AcceptConfuse(other);
        }

        private void RemoveEffect()
        {
            cancelled = true;
            level.Player = player;
            level.Board.SetObject(Position, level.Player);
        }

        public override void PutOff(string inventoryType)
        {
            player.PutOff(inventoryType);
        }

        public override void PutOn(string inventoryType)
        {
            player.PutOn(inventoryType);
        }

        public override List<InventoryItem> GetInventory()
        {
            return player.GetInventory();
        }

        public override List<InventoryItem> GetAppliedInventory()
        {
            return player.GetAppliedInventory();
        }

        public override string GetType()
        {
            return PlayerType.ConfusedPlayer;
        }
    }
}
