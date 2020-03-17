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

            var key = Keyboard.GetKey();
            if (key == ConsoleKey.RightArrow)
            {
                deltaX = 1;
            }
            else if (key == ConsoleKey.LeftArrow)
            {
                deltaX = -1;
            } 
            else if (key == ConsoleKey.DownArrow)
            {
                deltaY = 1;
            }
            else if (key == ConsoleKey.UpArrow)
            {
                deltaY = -1;
            }

            if (deltaY != 0 || deltaX != 0)
            {
                moveInteractor.IntentMove(deltaY, deltaX);
            }
        }
    }
}