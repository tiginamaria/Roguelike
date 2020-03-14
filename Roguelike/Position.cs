namespace Roguelike
{
    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int y, int x)
        {
            X = x;
            Y = y;
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