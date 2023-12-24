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

        public (BigInteger x, BigInteger y, BigInteger z) PosVelCrossProduct()
        {
            return (Pos.y * Vel.z - Pos.z * Vel.y, Pos.z * Vel.x - Pos.x * Vel.z, Pos.x * Vel.y - Pos.y * Vel.x);
        }

        public static (BigInteger x, BigInteger y, BigInteger z) Subtract((BigInteger x, BigInteger y, BigInteger z) a, (BigInteger x, BigInteger y, BigInteger z) b)
        {
            return (a.x - b.x, a.y - b.y, a.z - b.z);
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

        private (BigInteger[,] A, BigInteger[] b) BuildLinearSystem()
        {
            (BigInteger A0x, BigInteger A0y, BigInteger A0z) = Hailstone.Subtract(_hailstones[0].Vel, _hailstones[1].Vel);
            (BigInteger A1x, BigInteger A1y, BigInteger A1z) = Hailstone.Subtract(_hailstones[0].Vel, _hailstones[2].Vel);

            (BigInteger B0x, BigInteger B0y, BigInteger B0z) = Hailstone.Subtract(_hailstones[1].Pos, _hailstones[0].Pos);
            (BigInteger B1x, BigInteger B1y, BigInteger B1z) = Hailstone.Subtract(_hailstones[2].Pos, _hailstones[0].Pos);

            (BigInteger C0x, BigInteger C0y, BigInteger C0z) = Hailstone.Subtract(_hailstones[1].PosVelCrossProduct(), _hailstones[0].PosVelCrossProduct());
            (BigInteger C1x, BigInteger C1y, BigInteger C1z) = Hailstone.Subtract(_hailstones[2].PosVelCrossProduct(), _hailstones[0].PosVelCrossProduct());


            BigInteger[,] A = new BigInteger[6, 6]
            {
                {0, -A0z, A0y, 0, -B0z, B0y},
                {A0z, 0, -A0x, B0z, 0, -B0x},
                {-A0y, A0x, 0, -B0y, B0x, 0},
                {0, -A1z, A1y, 0, -B1z, B1y},
                {A1z, 0, -A1x, B1z, 0, -B1x},
                {-A1y, A1x, 0, -B1y, B1x, 0}
            };

            BigInteger[] b = new BigInteger[6] { C0x, C0y, C0z, C1x, C1y, C1z };
            return (A, b);
        }

        private static BigInteger[] SolveLinearSystem(BigInteger[,] A, BigInteger[] b)
        {
            // Solve system using Gaussian elimination
            int n = b.Length;
            for (int p = 0; p < n; p++)
            {
                // Find pivot row and swap
                int max = p;
                for (int i = p + 1; i < n; i++)
                {
                    if (BigInteger.Abs(A[i, p]) > BigInteger.Abs(A[max, p]))
                    {
                        max = i;
                    }
                }

                BigInteger[] temp = new BigInteger[n];
                for (int i = 0; i < n; i++)
                {
                    temp[i] = A[p, i];
                    A[p, i] = A[max, i];
                    A[max, i] = temp[i];
                }

                (b[max], b[p]) = (b[p], b[max]);

                // Singular or nearly singular
                if (BigInteger.Abs(A[p, p]) <= 0)
                {
                    throw new Exception("Matrix is singular or nearly singular");
                }

                // Pivot within A and b
                for (int i = p + 1; i < n; i++)
                {
                    BigInteger alpha = A[i, p];
                    BigInteger beta = A[p, p];
                    BigInteger gcd = BigInteger.GreatestCommonDivisor(alpha, beta);
                    alpha /= gcd;
                    beta /= gcd;
                    b[i] = beta * b[i] - alpha * b[p];
                    for (int j = p; j < n; j++)
                    {
                        A[i, j] = beta * A[i, j] - alpha * A[p, j];
                    }
                }
            }

            // Back substitution
            BigInteger[] x = new BigInteger[n];
            for (int i = n - 1; i >= 0; i--)
            {
                BigInteger sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += A[i, j] * x[j];
                }

                x[i] = (b[i] - sum) / A[i, i];
            }
            return x;
        }

        public Hailstone RockThatHitsAllHailstones()
        {
            (BigInteger[,] A, BigInteger[] b) = BuildLinearSystem();
            BigInteger[] x = SolveLinearSystem(A, b);
            return new Hailstone((x[0], x[1], x[2]), (x[3], x[4], x[5]));
        }
    }
}