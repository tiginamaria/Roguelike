using System;
using Roguelike.Interaction;

namespace Roguelike.Input
{
    /// <summary>
    /// A controller to handle exit button click.
    /// Notifies the target if occurred.
    /// </summary>
    public class ExitGameProcessor : IInputProcessor
    {
        private readonly ExitGameInteractor interactor; 
            
        public ExitGameProcessor(ExitGameInteractor interactor)
        {
            this.interactor = interactor;
        }

        public void ProcessInput(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                interactor.Exit();
            }
        }
    }
}