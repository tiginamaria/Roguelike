using System;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input.Processors
{
    public class InventoryProcessor: IInputProcessor
    {
        private readonly InventoryInteractor inventoryInteractor;
        
        public InventoryProcessor(InventoryInteractor inventoryInteractor)
        {
            this.inventoryInteractor = inventoryInteractor;
        }
        
        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            if (keyInfo.Key == ConsoleKey.F && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseForceItem);
                return;
            }
            if (keyInfo.Key == ConsoleKey.H && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseHealthItem);
                return;
            }
            if (keyInfo.Key == ConsoleKey.E && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseExperienceItem);
                return;
            }
            if (keyInfo.Key == ConsoleKey.A && keyInfo.Modifiers == ConsoleModifiers.Alt)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseAllItem);
                return;
            }
            
            if (keyInfo.Key == ConsoleKey.F)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseForceItem);
            }
            if (keyInfo.Key == ConsoleKey.H)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseHealthItem);
            }
            if (keyInfo.Key == ConsoleKey.E)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseExperienceItem);
            }
            if (keyInfo.Key == ConsoleKey.A)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseAllItem);
            }
        }
    }
}