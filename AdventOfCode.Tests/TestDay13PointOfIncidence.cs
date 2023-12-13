namespace AdventOfCode.Tests
{
    public class TestDay13PointOfIncidence
    {
        private static string RowToCol(string input)
        {
            return string.Join(Environment.NewLine, input.ToCharArray());
        }
        [Fact]
        public void TestReflectionLineForRowWithNoSymmetryIsMinusOne()
        {
            string rowInput = ".#";
            PointOfIncidence rowIncidence = new(rowInput);
            Assert.Equal(-1, rowIncidence.ColumnMirrorIdx());

            string colInput = RowToCol(rowInput);
            PointOfIncidence colIncidence = new(colInput);
            Assert.Equal(-1, colIncidence.RowMirrorIdx());
        }

        [Fact]
        public void TestReflectionLineForRowWithOddSymmetryIsMinusOne()
        {
            string rowInput = ".#.";
            PointOfIncidence rowIncidence = new(rowInput);
            Assert.Equal(-1, rowIncidence.ColumnMirrorIdx());

            string colInput = RowToCol(rowInput);
            PointOfIncidence colIncidence = new(colInput);
            Assert.Equal(-1, colIncidence.RowMirrorIdx());
        }

        [Fact]
        public void TestReflectionLineForRowWithEvenSymmetryIsMiddleLine()
        {
            string rowInput = ".##.";
            PointOfIncidence rowIncidence = new(rowInput);
            Assert.Equal(1, rowIncidence.ColumnMirrorIdx());

            string colInput = RowToCol(rowInput);
            PointOfIncidence colIncidence = new(colInput);
            Assert.Equal(1, colIncidence.RowMirrorIdx());
        }

        [Fact]
        public void TestPartiallyMirroredRowsCount()
        {
            string rowInput = ".##..";
            PointOfIncidence rowIncidence = new(rowInput);
            Assert.Equal(1, rowIncidence.ColumnMirrorIdx());

            string colInput = RowToCol(rowInput);
            PointOfIncidence colIncidence = new(colInput);
            Assert.Equal(1, colIncidence.RowMirrorIdx());
        }

        [Fact]
        public void TestCanDetectReflectionInTwoDimensionalPatterns()
        {
            string inputA = @"#.##..##.
                              ..#.##.#.
                              ##......#
                              ##......#
                              ..#.##.#.
                              ..##..##.
                              #.#.##.#.";

            PointOfIncidence incidenceA = new(inputA);
            Assert.Equal(4, incidenceA.ColumnMirrorIdx());
            Assert.Equal(-1, incidenceA.RowMirrorIdx());

            string inputB = @"#...##..#
                              #....#..#
                              ..##..###
                              #####.##.
                              #####.##.
                              ..##..###
                              #....#..#";

            PointOfIncidence incidenceB = new(inputB);
            Assert.Equal(-1, incidenceB.ColumnMirrorIdx());
            Assert.Equal(3, incidenceB.RowMirrorIdx());
        }

        [Fact]
        public void TestCanDetectReflectionWithMismatches()
        {
            string inputA = @"#.##..##.
                              ..#.##.#.
                              ##......#
                              ##......#
                              ..#.##.#.
                              ..##..##.
                              #.#.##.#.";

            PointOfIncidence incidenceA = new(inputA);
            Assert.Equal(-1, incidenceA.ColumnMirrorIdx(numMismatches: 1));
            Assert.Equal(2, incidenceA.RowMirrorIdx(numMismatches: 1));

            string inputB = @"#...##..#
                              #....#..#
                              ..##..###
                              #####.##.
                              #####.##.
                              ..##..###
                              #....#..#";

            PointOfIncidence incidenceB = new(inputB);
            Assert.Equal(-1, incidenceB.ColumnMirrorIdx(numMismatches: 1));
            Assert.Equal(0, incidenceB.RowMirrorIdx(numMismatches: 1));
        }
    }
}