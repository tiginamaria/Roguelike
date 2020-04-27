using System;
using Roguelike.Interaction;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// A controller to handle exit button click.
    /// Notifies the target if occurred.
    /// </summary>
    public class ExitGameProcessor : IInputProcessor
    {
        private readonly ExitGameInteractor exitInteractor;
        private readonly SaveGameInteractor saveGameInteractor;

        public ExitGameProcessor(ExitGameInteractor exitInteractor, SaveGameInteractor saveGameInteractor)
        {
            this.exitInteractor = exitInteractor;
            this.saveGameInteractor = saveGameInteractor;
        }

        public void ProcessInput(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                saveGameInteractor.Save();
                saveGameInteractor.Dump();
                exitInteractor.Exit();
            }
        }
    }
}
