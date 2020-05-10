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

        public void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.S)
            {
                interactor.Save();
            }
        }
    }
}