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

        public GameObject GetObject(int y, int x)
        {
            return gameObjects[y, x];
        }

        public bool IsWall(int y, int x)
        {
            return gameObjects[y, x] is Wall;
        }
        
        public bool IsEmpty(int y, int x)
        {
            return gameObjects[y, x] is EmptyCell;
        }

        public void SetObject(GameObject gameObject, int y, int x)
        {
            gameObjects[y, x] = gameObject;
        }
    }
}