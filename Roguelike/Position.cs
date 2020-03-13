namespace Roguelike
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int y, int x)
        {
            X = x;
            Y = y;
        }

        public void Change(int deltaY, int deltaX)
        {
            X += deltaX;
            Y += deltaY;
        }
        
        public Position Add(int deltaY, int deltaX)
        {
            return new Position(Y + deltaY, X + deltaX);
        }
        
        public static bool operator ==(Position first, Position second)
        {
            return first.X == second.X && first.Y == second.Y;
        }
        
        public static bool operator !=(Position first, Position second)
        {
            return first.X != second.X || first.Y != second.Y;
        }
    }
}