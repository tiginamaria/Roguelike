using System;

namespace Roguelike
{
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