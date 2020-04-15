using System;
using Roguelike.Interaction;

namespace Roguelike.Input
{
    /// <summary>
    /// A processor to handle player move actions.
    /// </summary>
    public class MoveProcessor : IInputProcessor
    {
        private readonly MoveInteractor moveInteractor;
        
        public MoveProcessor(MoveInteractor moveInteractor)
        {
            this.moveInteractor = moveInteractor;
        }
        
        public void ProcessInput(ConsoleKey key)
        {
            var deltaX = 0;
            var deltaY = 0;
            
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
