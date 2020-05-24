using System;
using Roguelike.Interaction;
using Roguelike.Model;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// Handles a saving game input and passes it to the interactor.
    /// </summary>
    public class SaveGameProcessor: IInputProcessor
    {
        private readonly SaveGameInteractor interactor;

        public SaveGameProcessor(SaveGameInteractor interactor) => this.interactor = interactor;

        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            if (keyInfo.Key == ConsoleKey.S)
            {
                interactor.Save(character);
            }
        }
    }
}