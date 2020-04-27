using System;
using Roguelike.Interaction;

namespace Roguelike.Input.Processors
{
    public class SaveGameProcessor: IInputProcessor
    {

        private readonly SaveGameInteractor interactor;

        public SaveGameProcessor(SaveGameInteractor interactor)
        {
            this.interactor = interactor;
        }

        public void ProcessInput(ConsoleKey key)
        {
            if (key == ConsoleKey.S)
            {
                interactor.Save();
            }
        }
    }
}