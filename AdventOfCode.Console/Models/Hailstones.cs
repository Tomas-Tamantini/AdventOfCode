using System.Numerics;

namespace AdventOfCode.Console.Models
{
    public record Hailstone((BigInteger x, BigInteger y, BigInteger z) Pos, (BigInteger x, BigInteger y, BigInteger z) Vel)
    {
        public BigInteger XYLinearConstant => Vel.y * Pos.x - Vel.x * Pos.y;
        public double TimeToReachXY(double x, double y)
        {
            BigInteger dx = (BigInteger)x - Pos.x;
            if (dx != 0) return (double)dx / (double)Vel.x;
            BigInteger dy = (BigInteger)y - Pos.y;
            return dy == 0 ? 0 : (double)dy / (double)Vel.y;
        }
    };

    public record BoundingBox((BigInteger x, BigInteger y) Min, (BigInteger x, BigInteger y) Max)
    {
        public bool Contains((double x, double y) point)
        {
            return point.x >= (double)Min.x && point.x <= (double)Max.x && point.y >= (double)Min.y && point.y <= (double)Max.y;
        }
    }

    public class Hailstones
    {

        private readonly List<Hailstone> _hailstones;
        private readonly BoundingBox boundingBox;

        public Hailstones(IEnumerable<Hailstone> hailstones, BoundingBox boundingBox)
        {
            this.boundingBox = boundingBox;
            _hailstones = hailstones.ToList();
        }

        public static (double, double) XYIntersection(Hailstone h1, Hailstone h2)
        {
            BigInteger denominator = h1.Vel.x * h2.Vel.y - h2.Vel.x * h1.Vel.y;
            if (denominator == 0) return (double.NaN, double.NaN);
            BigInteger numeratorX = h1.Vel.x * h2.XYLinearConstant - h2.Vel.x * h1.XYLinearConstant;
            BigInteger numeratorY = h1.Vel.y * h2.XYLinearConstant - h2.Vel.y * h1.XYLinearConstant;
            return ((double)numeratorX / (double)denominator, (double)numeratorY / (double)denominator);
        }

        public static bool XYPathsAreCoincident(Hailstone h1, Hailstone h2)
        {
            BigInteger denominator = h1.Vel.x * h2.Vel.y - h2.Vel.x * h1.Vel.y;
            if (denominator != 0) return false;
            BigInteger dx = h1.Pos.x - h2.Pos.x;
            BigInteger dy = h1.Pos.y - h2.Pos.y;
            return dx * h1.Vel.y == dy * h1.Vel.x;
        }

        public bool XYPathsWillCrossWithinBoundingBoxInTheFuture(Hailstone h1, Hailstone h2)
        {
            if (XYPathsAreCoincident(h1, h2))
            {
                // TODO: Implement and unskip corresponding test
                throw new NotImplementedException();
            }
            (double x, double y) = XYIntersection(h1, h2);
            if (double.IsNaN(x) || double.IsNaN(y)) return false;
            if (!boundingBox.Contains((x, y))) return false;
            double t1 = h1.TimeToReachXY(x, y);
            double t2 = h2.TimeToReachXY(x, y);
            if (t1 < 0 || t2 < 0) return false;
            return true;
        }

        private IEnumerable<(int, int)> IndicesPairs()
        {
            for (int i = 0; i < _hailstones.Count - 1; i++)
            {
                for (int j = i + 1; j < _hailstones.Count; j++)
                {
                    yield return (i, j);
                }
            }
        }

        public int NumFutureXYIntersections()
        {
            return IndicesPairs().Count(pair => XYPathsWillCrossWithinBoundingBoxInTheFuture(_hailstones[pair.Item1], _hailstones[pair.Item2]));
        }
    }
}