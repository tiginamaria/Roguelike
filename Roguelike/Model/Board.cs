using System;

namespace Roguelike.Model
{
    /// <summary>
    /// Represents a game board with Game Objects on it.
    /// </summary>
    public class Board
    {
        public int Width { get; }
        public int Height { get; }

        private GameObject[,] gameObjects;
        
        public Board(int width, int height, GameObject[,] objects)
        {
            Width = width;
            Height = height;
            gameObjects = objects;
        }

        public GameObject GetObject(Position position) => gameObjects[position.Y, position.X];

        public bool IsWall(Position position) => gameObjects[position.Y, position.X] is Wall;

        public bool IsEmpty(Position position) => gameObjects[position.Y, position.X] is EmptyCell;

        /// <summary>
        /// Moves an object from one position to another,
        /// inserting an Empty Cell to the old position.
        /// </summary>
        public void MoveObject(Position from, Position to)
        {
            if (from == to)
            {
                return;
            }
            gameObjects[to.Y, to.X] = gameObjects[from.Y, from.X];
            gameObjects[from.Y, from.X] = new EmptyCell(from);
        }

        /// <summary>
        /// Returns true iff the given position is inside the board dimensions.
        /// </summary>
        public bool CheckOnBoard(Position position)
        {
            return position.X >= 0 && position.X < Width &&
                   position.Y >= 0 && position.Y < Height;
        }
        
        public Player FindPlayer()
        {
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    var position = new Position(row, col);
                    var gameObject = GetObject(position);
                    var player = gameObject as Player;
                    if (player != null)
                    {
                        return player;
                    }
                }
            }
            throw new Exception("Player not found.");
        }
    }
}
