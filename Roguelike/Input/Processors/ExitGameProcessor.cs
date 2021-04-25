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

        public ExitGameProcessor(ExitGameInteractor exitInteractor)
        {
            this.exitInteractor = exitInteractor;
        }

        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                exitInteractor.Exit(character);
            }
        }
    }
}
