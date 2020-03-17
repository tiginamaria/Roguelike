namespace Roguelike
{
    /// <summary>
    /// Represents a rectangle with fixed dimensions.
    /// Width and Height are set in the constructor.
    /// While changing coordinates, the opposite one shifts automatically.
    /// </summary>
    public class FixedBoundRectangle
    {
        private int left;
        private int right;
        private int top;
        private int bottom;
        
        public int Left
        {
            get { return left; }
            set
            {
                left = value;
                right = left + Width;
            }
        }

        public int Right
        {
            get { return right; }
            set
            {
                right = value;
                left = right - Width;
            }
        }

        public int Top
        {
            get { return top; }
            set
            {
                top = value;
                bottom = top + Height;
            }
        }

        public int Bottom
        {
            get { return bottom; }
            set
            {
                bottom = value;
                top = bottom - Height;
            }
        }

        public int Width { get; }
        public int Height { get; }

        public FixedBoundRectangle(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            right = left + width;
            bottom = top + height;
            Height = height;
            Width = width;
        }
    }
}