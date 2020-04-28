using System;
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
        
        public void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.F && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(InventoryType.IncreaseForceItem);
                return;
            }
            if (keyInfo.Key == ConsoleKey.H && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(InventoryType.IncreaseHealthItem);
                return;
            }
            if (keyInfo.Key == ConsoleKey.E && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(InventoryType.IncreaseExperienceItem);
                return;
            }
            if (keyInfo.Key == ConsoleKey.A && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(InventoryType.IncreaseAllItem);
                return;
            }
            
            if (keyInfo.Key == ConsoleKey.F)
            {
                inventoryInteractor.PutOn(InventoryType.IncreaseForceItem);
            }
            if (keyInfo.Key == ConsoleKey.H)
            {
                inventoryInteractor.PutOn(InventoryType.IncreaseHealthItem);
            }
            if (keyInfo.Key == ConsoleKey.E)
            {
                inventoryInteractor.PutOn(InventoryType.IncreaseExperienceItem);
            }
            if (keyInfo.Key == ConsoleKey.A)
            {
                inventoryInteractor.PutOn(InventoryType.IncreaseAllItem);
            }
        }
    }
}