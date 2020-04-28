using System;
using System.Security.Policy;
using Roguelike.Interaction;
using Roguelike.Model.Inventory;

namespace Roguelike.Input.Processors
{
    public class InventoryProcessor: IInputProcessor
    {
        private readonly InventoryInteractor inventoryInteractor;
        
        public InventoryProcessor(InventoryInteractor inventoryInteractor)
        {
            this.inventoryInteractor = inventoryInteractor;
        }
        
        public void ProcessInput(ConsoleKey key)
        {
            if (key == ConsoleKey.F)
            {
                inventoryInteractor.PutOn(InventoryType.IncreaseForceItem);
            }
            if (key == ConsoleKey.R)
            {
                inventoryInteractor.PutOn(InventoryType.IncreaseForceItem);
            }
        }
    }
}