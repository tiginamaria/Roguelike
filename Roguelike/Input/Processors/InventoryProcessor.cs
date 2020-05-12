using System;
using Roguelike.Input.Controllers;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input.Processors
{
    public class InventoryProcessor: IInputProcessor
    {
        private readonly InventoryInteractor inventoryInteractor;
        private readonly IActionListener listener;

        public InventoryProcessor(InventoryInteractor inventoryInteractor, IActionListener listener = null)
        {
            this.inventoryInteractor = inventoryInteractor;
            this.listener = listener;
        }
        
        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            if (keyInfo.Key == ConsoleKey.F && keyInfo.Modifiers == ConsoleModifiers.Shift)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseForceItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffForce);
                return;
            }
            if (keyInfo.Key == ConsoleKey.H && keyInfo.Modifiers == ConsoleModifiers.Shift)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseHealthItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffHealth);
                return;
            }
            if (keyInfo.Key == ConsoleKey.E && keyInfo.Modifiers == ConsoleModifiers.Shift)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseExperienceItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffExperience);
                return;
            }
            if (keyInfo.Key == ConsoleKey.A && keyInfo.Modifiers == ConsoleModifiers.Shift)
            {
                inventoryInteractor.PutOff(character, InventoryType.IncreaseAllItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffAll);
                return;
            }
            
            if (keyInfo.Key == ConsoleKey.F)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseForceItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyForce);
            }
            if (keyInfo.Key == ConsoleKey.H)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseHealthItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyHealth);
            }
            if (keyInfo.Key == ConsoleKey.E)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseExperienceItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyExperience);
            }
            if (keyInfo.Key == ConsoleKey.A)
            {
                inventoryInteractor.PutOn(character, InventoryType.IncreaseAllItem);
                listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyAll);
            }
        }
    }
}