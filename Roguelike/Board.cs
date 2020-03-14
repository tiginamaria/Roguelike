using System;

namespace Roguelike
{
    public class Board
    {
        public int Width { get; }
        public int Height { get; }
        
        public event EventHandler OnChange;

        private GameObject[,] gameObjects;
        
        public Board(int width, int height, GameObject[,] objects)
        {
            Width = width;
            Height = height;
            gameObjects = objects;
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

        public void MoveObject(Position from, Position to)
        {
            if (from == to)
            {
                return;
            }
            gameObjects[to.Y, to.X] = gameObjects[from.Y, from.X];
            gameObjects[from.Y, from.X] = new EmptyCell(from);
            
            OnChange?.Invoke(this, null);
        }
    }
}