using System;

namespace Roguelike
{
    /// <summary>
    /// A controller to handle exit button click.
    /// Notifies the state manager if occurred.
    /// </summary>
    public class ExitGameController
    {
        private readonly StateManager stateManager;

        public ExitGameController()
        {
            stateManager = StateManager.GetInstance();
        }
        
        public void Update()
        {
            var key = Keyboard.GetKey();
            if (key == ConsoleKey.Escape)
            {
                stateManager.Exit();
            }
        }
    }
}