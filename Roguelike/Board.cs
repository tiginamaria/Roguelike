namespace Roguelike
{
    public class Board
    {
        public int Width { get; }
        public int Height { get; }

        private GameObject[,] gameObjects;
        
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            gameObjects = new GameObject[Height, Width];
        }

        public GameObject GetObject(Position position)
        {
            return gameObjects[position.Y, position.X];
        }

        public bool IsWall(Position position)
        {
            return gameObjects[position.Y, position.X] is Wall;
        }
        
        public bool IsEmpty(Position position)
        {
            return gameObjects[position.Y, position.X] is EmptyCell;
        }

        public void SetObject(GameObject gameObject, Position position)
        {
            gameObjects[position.Y, position.X] = gameObject;
        }

        public void MoveObject(Position from, Position to)
        {
            if (from == to)
            {
                return;
            }
            gameObjects[to.Y, to.X] = gameObjects[from.Y, from.X];
            gameObjects[from.Y, from.X] = new EmptyCell(from);
        }
    }
}