using System;

namespace Roguelike
{
    public class MoveController : IInputController
    {
        private Player player;

        public MoveController(Player player)
        {
            this.player = player;
        }
        
        public void Update()
        {
            if (Console.ReadKey().Key == ConsoleKey.RightArrow)
            {
                player.IntentMoveRight();
            }
            
            if (Console.ReadKey().Key == ConsoleKey.LeftArrow)
            {
                player.IntentMoveLeft();
            }
            
            if (Console.ReadKey().Key == ConsoleKey.DownArrow)
            {
                player.IntentMoveDown();
            }
            
            if (Console.ReadKey().Key == ConsoleKey.UpArrow)
            {
                player.IntentMoveUp();
            }
        }
    }
}