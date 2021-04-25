using System;
using Roguelike.Interaction;
using Roguelike.Model;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// A processor to handle player move actions.
    /// </summary>
    public class MoveProcessor : IInputProcessor
    {
        private readonly PlayerMoveInteractor moveInteractor;
        
        public MoveProcessor(PlayerMoveInteractor moveInteractor) => this.moveInteractor = moveInteractor;

        public void ProcessInput(ConsoleKeyInfo keyInfo, Character character)
        {
            var deltaX = 0;
            var deltaY = 0;
            
            if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                deltaX = 1;
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                deltaX = -1;
            } 
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                deltaY = 1;
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                deltaY = -1;
            }

            if (deltaY != 0 || deltaX != 0)
            {
                moveInteractor.IntentMove(character, deltaY, deltaX);
            }
        }
    }
}
