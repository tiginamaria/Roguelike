namespace Roguelike
{
    public class Position
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
    }
}