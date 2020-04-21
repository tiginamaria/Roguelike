using System.Collections.Generic;
using Roguelike.Model.Objects;

namespace Roguelike.Model
{
    /// <summary>
    /// Represents a graph, built by the given board.
    /// Precalculates all pairwise distances between board cells
    /// using the BFS algorithm.
    /// </summary>
    public class BoardGraph
    {
        private static readonly int[] Dx = {0, 0, -1, 1};
        private static readonly int[] Dy = {-1, 1, 0, 0};

        private const int MovesNumber = 4;

        private readonly Board board;
        private readonly int[,] distance;
        private readonly int n;

        public BoardGraph(Board board)
        {
            this.board = board;
            n = board.Height * board.Width;
            distance = new int[n, n];
            CalculateDistances();
        }

        public int GetDistance(Position from, Position to)
        {
            return distance[PositionToId(from), PositionToId(to)];
        }

        private void CalculateDistances()
        {
            for (var y = 0; y < board.Height; y++)
            {
                for (var x = 0; x < board.Width; x++)
                {
                    Bfs(new Position(y, x));
                }
            }
        }

        private int PositionToId(Position position)
        {
            return position.Y * board.Width + position.X;
        }

        private void Bfs(Position v)
        {
            var vId = PositionToId(v);
            for (var i = 0; i < n; i++)
            {
                distance[vId, i] = -1;
            }

            if (board.IsWall(v))
            {
                return;
            }

            var queue = new Queue<Position>();
            distance[vId, vId] = 0;
            queue.Enqueue(v);
            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                var uId = PositionToId(u);
                for (var i = 0; i < MovesNumber; i++)
                {
                    var z = u + new Position(Dy[i], Dx[i]);
                    var zId = PositionToId(z);
                    if (board.CheckOnBoard(z) && !board.IsWall(z) && distance[vId, zId] == -1)
                    {
                        queue.Enqueue(z);
                        distance[vId, zId] = distance[vId, uId] + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Return the best empty position next to 'from' in order to go to 'to'.
        /// </summary>
        public Position Nearest(Position from, Position to)
        {
            var fromId = PositionToId(from);
            var toId = PositionToId(to);
            if (!board.CheckOnBoard(from) || !board.CheckOnBoard(to))
            {
                return from;
            }

            var bestDistance = distance[fromId, toId];
            if (bestDistance == -1)
            {
                return from;
            }

            var bestPosition = from;
            for (var i = 0; i < MovesNumber; i++)
            {
                var z = from + new Position(Dy[i], Dx[i]);
                var zId = PositionToId(z);
                if (board.CheckOnBoard(z) && distance[toId, zId] < bestDistance && !board.IsWall(z))
                {
                    bestDistance = distance[toId, zId];
                    bestPosition = z;
                }
            }

            return bestPosition;
        }

        /// <summary>
        /// Return the best empty position next to 'from' in order to go from 'to'.
        /// </summary>
        public Position Farthest(Position from, Position to)
        {
            var fromId = PositionToId(from);
            var toId = PositionToId(to);
            var bestDistance = distance[fromId, toId];
            if (bestDistance == -1)
            {
                return from;
            }

            var bestPosition = from;
            for (var i = 0; i < MovesNumber; i++)
            {
                var z = from + new Position(Dy[i], Dx[i]);
                var zId = PositionToId(z);
                if (board.CheckOnBoard(z) && distance[toId, zId] > bestDistance && !board.IsWall(z))
                {
                    bestDistance = distance[toId, zId];
                    bestPosition = z;
                }
            }

            return bestPosition;
        }
    }
}
