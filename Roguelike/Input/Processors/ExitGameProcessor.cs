using System;
using Roguelike.Interaction;
using Roguelike.Model;

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

        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                saveGameInteractor.Save(character);
                saveGameInteractor.Dump();
                exitInteractor.Exit(character);
            }
        }
    }
}
