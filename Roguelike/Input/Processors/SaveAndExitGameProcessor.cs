using System;
using Roguelike.Interaction;
using Roguelike.Model;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// A controller to handle exit button click.
    /// Notifies the target if occurred.
    /// </summary>
    public class SaveAndExitGameProcessor : IInputProcessor
    {
        private readonly ExitGameInteractor exitInteractor;
        private readonly SaveGameInteractor saveInteractor;

        public SaveAndExitGameProcessor(ExitGameInteractor exitInteractor, SaveGameInteractor saveInteractor)
        {
            this.saveInteractor = saveInteractor;
            this.exitInteractor = exitInteractor;
        }

        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                saveInteractor.Save();
                saveInteractor.Dump();
                exitInteractor.Exit(character);
            }
        }
    }
}
