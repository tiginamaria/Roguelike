namespace Roguelike
{
    public abstract class Character : GameObject
    {
        public int IntentHorizontalMove { get; private set; }
        public int IntentVerticalMove { get; private set; }

        public Character(Position startPosition) : base(startPosition)
        {
        }

        public void ClearMoveIntent()
        {
            IntentVerticalMove = 0;
            IntentHorizontalMove = 0;
        }

        public void IntentMoveRight()
        {
            IntentHorizontalMove = 1;
        }
        
        public void IntentMoveLeft()
        {
            IntentHorizontalMove = -1;
        }
        
        public void IntentMoveUp()
        {
            IntentVerticalMove = -1;
        }
        
        
        public void IntentMoveDown()
        {
            IntentVerticalMove = 1;
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

        public virtual void Update(Board board)
        {
        }
    }
}