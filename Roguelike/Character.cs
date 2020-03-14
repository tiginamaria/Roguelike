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
    }
}