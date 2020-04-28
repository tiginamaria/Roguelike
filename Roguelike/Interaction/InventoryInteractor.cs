﻿using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    public class InventoryInteractor
    {
        private IPlayView playView;
        private Level level;

        public InventoryInteractor(Level level, IPlayView playView)
        {
            this.level = level;
            this.playView = playView;
        }

        public void PutOn(string inventoryType)
        {
            level.Player.PutOn(inventoryType);
            playView.UpdateInventory(level);
        }

        public void PutOff(string inventoryType)
        {
            level.Player.PutOff(inventoryType);
            playView.UpdateInventory(level);
        }
    }
}