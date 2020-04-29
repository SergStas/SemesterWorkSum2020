using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Controls;

namespace SimpleGeneticCode
{
    static class PointExtensions
    {
        public static IEnumerable<Point> GetNeighbours(this Point p)
        {
            int[] d = { -1, 0, 1 };
            return d.SelectMany(x => d.Select(a => new Point(p.X + x, p.Y + a)))
                .Where(a => !(a.X == p.X && a.Y == p.Y));
        }

        public static IEnumerable<Point> GetRound(this Point p)
        {
            int[] d = { -1, 0, 1 };
            return d.SelectMany(x => d.Select(a => new Point(p.X + x, p.Y + a)))
                .Where(a => Math.Abs(a.X - p.X) > 0 || Math.Abs(a.Y - p.Y) > 0);
        }

        public static Point Move(this Point pos, int dx, int dy)
        {
            return new Point(pos.X + dx, pos.Y + dy);
        }
    }
}