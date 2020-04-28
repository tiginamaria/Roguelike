using System;
using System.Security.Policy;
using Roguelike.Interaction;

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
                inventoryInteractor.PutOn();
            }
            if (key == ConsoleKey.F)
            {
                inventoryInteractor.PutOff();
            }
        }
    }
}