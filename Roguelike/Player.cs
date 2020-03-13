namespace Roguelike
{
    public class Player : Character
    {
        public Player(int startY, int startX) : base(startY, startX)
        {
        }

        public void MoveUp()
        {
            Position.Change(-1, 0);
        }
        
        public void MoveDown()
        {
            Position.Change(1, 0);
        }
        
        public void MoveLeft()
        {
            Position.Change(0, -1);
        }
        
        public void MoveRight()
        {
            Position.Change(0, 1);
        }
    }
}