namespace Roguelike.Model
{
    /// <summary>
    /// Represents the position in a grid with integer nodes.
    /// </summary>
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int y, int x)
        {
            X = x;
            Y = y;
        }
        
        public static Position operator +(Position first, Position second) => 
            new Position(first.Y + second.Y, first.X + second.X);
        
        public static Position operator -(Position first, Position second) => 
            new Position(first.Y - second.Y, first.X - second.X);

        public static bool operator ==(Position first, Position second)
        {
            return first.X == second.X && first.Y == second.Y;
        }
        
        public static bool operator !=(Position first, Position second)
        {
            return first.X != second.X || first.Y != second.Y;
        }
        
        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}
