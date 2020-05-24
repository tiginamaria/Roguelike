using System;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Inventory;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// Handles inventory actions.
    /// </summary>
    public class InventoryProcessor: IInputProcessor
    {
        private readonly InventoryInteractor inventoryInteractor;
        private readonly IActionListener listener;

        public InventoryProcessor(InventoryInteractor inventoryInteractor, IActionListener listener = null)
        {
            this.inventoryInteractor = inventoryInteractor;
            this.listener = listener;
        }
        
        /// <summary>
        /// Pass the action to the appropriate interactor.
        /// </summary>
        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.F when keyInfo.Modifiers == ConsoleModifiers.Shift:
                    inventoryInteractor.PutOff(character, InventoryType.IncreaseForceItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffForce);
                    break;
                case ConsoleKey.H when keyInfo.Modifiers == ConsoleModifiers.Shift:
                    inventoryInteractor.PutOff(character, InventoryType.IncreaseHealthItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffHealth);
                    return;
                case ConsoleKey.E when keyInfo.Modifiers == ConsoleModifiers.Shift:
                    inventoryInteractor.PutOff(character, InventoryType.IncreaseExperienceItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffExperience);
                    break;
                case ConsoleKey.A when keyInfo.Modifiers == ConsoleModifiers.Shift:
                    inventoryInteractor.PutOff(character, InventoryType.IncreaseAllItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.TakeOffAll);
                    break;
                case ConsoleKey.F:
                    inventoryInteractor.PutOn(character, InventoryType.IncreaseForceItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyForce);
                    break;
                case ConsoleKey.H:
                    inventoryInteractor.PutOn(character, InventoryType.IncreaseHealthItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyHealth);
                    break;
                case ConsoleKey.E:
                    inventoryInteractor.PutOn(character, InventoryType.IncreaseExperienceItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyExperience);
                    break;
                case ConsoleKey.A:
                    inventoryInteractor.PutOn(character, InventoryType.IncreaseAllItem);
                    listener?.MakeAction(character as AbstractPlayer, ActionType.ApplyAll);
                    break;
            }
        }
    }
}