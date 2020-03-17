using System;
using System.Collections.Generic;

namespace Roguelike
{
    internal class Labyrinth
    {
        private readonly int height;
        private readonly int width;
        private readonly int n;
        private readonly bool[,] matrix;
        private static readonly int[] Dx = {0, 0, -1, 1};
        private static readonly int[] Dy = {-1, 1, 0, 0};

        public Labyrinth(int height, int width)
        {
            this.height = height;
            this.width = width;
            n = width * height;
            matrix = new bool[n, n];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    matrix[i, j] = false;
                }
            }

            Build();
        }

        private void Build()
        {
            var used = new bool[n];
            for (var i = 0; i < n; i++)
            {
                used[i] = false;
            }

            var usedNumber = n;
            var random = new Random();
            var x = random.Next(width);
            var y = random.Next(height);
            var v = Id(y, x);
            used[v] = true;
            usedNumber--;
            var neighbors = new List<int>();
            var stack = new Stack<int>();
            while (usedNumber > 0)
            {
                neighbors.Clear();
                for (var i = 0; i < 4; i++)
                {
                    var nx = x + Dx[i];
                    var ny = y + Dy[i];
                    if (!IsValidCell(ny, nx)) continue;
                    var u = Id(ny, nx);
                    if (!used[u])
                    {
                        neighbors.Add(u);
                    }
                }

                if (neighbors.Count == 0)
                {
                    v = stack.Pop();
                }
                else
                {
                    stack.Push(v);
                    var u = neighbors[random.Next(neighbors.Count)];
                    used[u] = true;
                    usedNumber--;
                    matrix[u, v] = matrix[v, u] = true;
                    v = u;
                }

                x = v % width;
                y = v / width;
            }
        }

        /// <summary>
        /// Checks is cells (y1, x1) and (y2, x2) are connected.
        /// Returns false if any coordinate is out of bounds.
        /// </summary>
        public bool AreConnected(int y1, int x1, int y2, int x2)
        {
            if (!(IsValidCell(y1, x1) && IsValidCell(y2, x2)))
            {
                return false;
            }

            var v = Id(y1, x1);
            var u = Id(y2, x2);
            return matrix[v, u];
        }

        private bool IsValidY(int y)
        {
            return 0 <= y && y < height;
        }

        private bool IsValidX(int x)
        {
            return 0 <= x && x < width;
        }

        public bool IsValidCell(int y, int x)
        {
            return IsValidX(x) && IsValidY(y);
        }

        private int Id(int y, int x)
        {
            return y * width + x;
        }
    }
}