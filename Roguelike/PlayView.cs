namespace Roguelike
{
    public class PlayView
    {
        public void Draw(Level level)
        {
            DrawBoard(level.Board);
        }

        private void DrawBoard(Board board)
        {
            var width = board.Width;
            var height = board.Height;

            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    DrawObject(board, new Position(row, col));
                }
            }
        }

        private void DrawObject(Board board, Position position)
        {
            
        }
    }
}