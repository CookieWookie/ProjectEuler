using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P015()
        {
            Grid grid = new Grid(20, 20);
            Console.WriteLine($"P015: {Grid.FindPaths(grid)}");
        }

        private class Grid
        {
            public Grid(int rows, int columns)
            {
                if (rows < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(rows));
                }
                if (columns < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(columns));
                }
                this.Rows = rows;
                this.Columns = columns;
            }

            public int Rows
            {
                get;
            }
            public int Columns
            {
                get;
            }
            public bool CanMoveDown => this.Rows > 0;
            public bool CanMoveRight => this.Columns > 0;

            public Grid MoveDown()
            {
                return new Grid(this.Rows - 1, this.Columns);
            }
            public Grid MoveRight()
            {
                return new Grid(this.Rows, this.Columns - 1);
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as Grid);
            }
            public bool Equals(Grid other)
            {
                if (other == null)
                {
                    return false;
                }
                return (other.Rows == this.Rows && other.Columns == this.Columns) ||
                    (other.Rows == this.Columns && other.Columns == this.Rows);
            }
            public override int GetHashCode()
            {
                return this.Rows + this.Columns;
            }

            public static long FindPaths(Grid grid)
            {
                long routes = 0;
                if (!routeCache.TryGetValue(grid, out routes))
                {
                    if (grid.CanMoveDown)
                    {
                        routes += FindPaths(grid.MoveDown());
                    }
                    if (grid.CanMoveRight)
                    {
                        routes += FindPaths(grid.MoveRight());
                    }
                    if (!grid.CanMoveDown && !grid.CanMoveRight)
                    {
                        routes += 1;
                    }
                    routeCache[grid] = routes;
                }
                return routes;
            }

            private static readonly Dictionary<Grid, long> routeCache = new Dictionary<Grid, long>();
        }
    }
}
