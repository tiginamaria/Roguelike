﻿namespace Roguelike.Model.Inventory
{
    /// <summary>
    /// Increases player's force.
    /// </summary>
    public class IncreaseForceItem : InventoryItem
    {
        public IncreaseForceItem(Position position, int forceEffect, int healthEffect, int experienceEffect) : 
            base(position, forceEffect, healthEffect, experienceEffect)
        {
        }

        public override string GetStringType() => InventoryType.IncreaseForceItem;
    }
}