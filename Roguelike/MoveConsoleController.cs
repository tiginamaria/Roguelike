using System;

namespace Roguelike
{
    public class MoveConsoleController
    {
        private MoveInteractor moveInteractor;
        
        public MoveConsoleController(MoveInteractor moveInteractor)
        {
            this.moveInteractor = moveInteractor;
        }
        
        public void Update()
        {
            var deltaX = 0;
            var deltaY = 0;
            
            if (Console.ReadKey().Key == ConsoleKey.RightArrow)
            {
                deltaX = 1;
            }
            else if (Console.ReadKey().Key == ConsoleKey.LeftArrow)
            {
                deltaX = -1;
            } 
            else if (Console.ReadKey().Key == ConsoleKey.DownArrow)
            {
                deltaY = 1;
            }
            else if (Console.ReadKey().Key == ConsoleKey.UpArrow)
            {
                deltaY = -1;
            }

            moveInteractor.IntentMove(deltaY, deltaX);
        }
    }
}