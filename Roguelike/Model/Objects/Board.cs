using System;
using System.Collections.Generic;
using Roguelike.Model.Inventory;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Model.Objects
{
    /// <summary>
    /// Represents a game board with Game Objects on it.
    /// </summary>
    public class Board
    {
        public int Width { get; }
        public int Height { get; }

        private readonly GameObject[,] gameObjects;

        public Board(int width, int height, GameObject[,] objects)
        {
            Width = width;
            Height = height;
            gameObjects = objects;
        }
        
        public GameObject[,] CloneGameObjects() => gameObjects.Clone() as GameObject[,];

        public GameObject GetObject(Position position) => gameObjects[position.Y, position.X];

        public bool IsWall(Position position) => gameObjects[position.Y, position.X] is Wall;

        public bool IsEmpty(Position position) => gameObjects[position.Y, position.X] is EmptyCell;

        public bool IsCharacter(Position position) => gameObjects[position.Y, position.X] is Character;
        public bool IsInventory(Position position) => gameObjects[position.Y, position.X] is InventoryItem;

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

        public void DeleteObject(Position position)
        {
            if (CheckOnBoard(position))
            {
                gameObjects[position.Y, position.X] = new EmptyCell(position);
            }
        }

        public List<AbstractPlayer> FindPlayers()
        {
            var result = new List<AbstractPlayer>();
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    var position = new Position(row, col);
                    var gameObject = GetObject(position);
                    if (gameObject is AbstractPlayer player)
                    {
                        result.Add(player);
                    }
                }
            }

            return result;
        }

        public List<Mob> FindMobs()
        {
            var mobs = new List<Mob>();
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    var position = new Position(row, col);
                    var gameObject = GetObject(position);
                    if (gameObject is Mob mob)
                    {
                        mobs.Add(mob);
                    }
                }
            }

            return mobs;
        }

        public void SetObject(Position position, GameObject gameObject) => 
            gameObjects[position.Y, position.X] = gameObject;
    }
}
