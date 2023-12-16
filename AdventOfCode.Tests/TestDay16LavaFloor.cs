namespace AdventOfCode.Tests
{
    public class TestDay16LavaFloor
    {
        const string contraptionInput = @".|...\....
                                          |.-.\.....
                                          .....|-...
                                          ........|.
                                          ..........
                                          .........\
                                          ..../.\\..
                                          .-.-/..|..
                                          .|....-|.\
                                          ..//.|....";
        private readonly LavaContraption lavaContraption = new(contraptionInput);

        [Fact]
        public void TestBeamPassingThroughEmptySpaceContinuesForward()
        {
            PhotonState initialBeam = new(0, 0, CardinalDirection.East);
            var nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Single(nextPhotons);
            Assert.Equal(new PhotonState(1, 0, CardinalDirection.East), nextPhotons.First());
        }

        [Fact]
        public void TestBeamGoingOutOfBoundsDoesNotGenerateNewPhoton()
        {
            PhotonState initialBeam = new(9, 0, CardinalDirection.East);
            var nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Empty(nextPhotons);
        }

        [Fact]
        public void TestBeamIsReflectedByMirrorsAt90Degrees()
        {
            PhotonState initialBeam = new(4, 1, CardinalDirection.West);
            var nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Single(nextPhotons);
            Assert.Equal(new PhotonState(4, 0, CardinalDirection.North), nextPhotons.First());

            initialBeam = new PhotonState(4, 6, CardinalDirection.South);
            nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Single(nextPhotons);
            Assert.Equal(new PhotonState(3, 6, CardinalDirection.West), nextPhotons.First());
        }

        [Fact]
        public void TestBeamAlignedWithSplitterIsNotSplit()
        {
            PhotonState initialBeam = new(1, 0, CardinalDirection.South);
            var nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Single(nextPhotons);
            Assert.Equal(new PhotonState(1, 1, CardinalDirection.South), nextPhotons.First());

            initialBeam = new PhotonState(2, 1, CardinalDirection.West);
            nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Single(nextPhotons);
            Assert.Equal(new PhotonState(1, 1, CardinalDirection.West), nextPhotons.First());
        }

        [Fact]
        public void TestBeamPerpendicularToSplitterGetsSplitInTwo()
        {
            PhotonState initialBeam = new(0, 1, CardinalDirection.West);
            var nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Equal(2, nextPhotons.Count());
            Assert.Contains(new PhotonState(0, 0, CardinalDirection.North), nextPhotons);
            Assert.Contains(new PhotonState(0, 2, CardinalDirection.South), nextPhotons);

            initialBeam = new PhotonState(2, 1, CardinalDirection.South);
            nextPhotons = lavaContraption.NextPhotons(initialBeam);
            Assert.Equal(2, nextPhotons.Count());
            Assert.Contains(new PhotonState(3, 1, CardinalDirection.East), nextPhotons);
            Assert.Contains(new PhotonState(1, 1, CardinalDirection.West), nextPhotons);
        }

        [Fact]
        public void TestContraptionCanHaveInitialPhotonsAllAroundItsEdge()
        {
            string contraptionInput = "..";
            LavaContraption lavaContraption = new(contraptionInput);
            IEnumerable<PhotonState> startingPhotons = lavaContraption.StartingPhotons();
            Assert.Equal(6, startingPhotons.Count());
            Assert.Contains(new PhotonState(0, 0, CardinalDirection.East), startingPhotons);
            Assert.Contains(new PhotonState(0, 0, CardinalDirection.South), startingPhotons);
            Assert.Contains(new PhotonState(0, 0, CardinalDirection.North), startingPhotons);
            Assert.Contains(new PhotonState(1, 0, CardinalDirection.West), startingPhotons);
            Assert.Contains(new PhotonState(1, 0, CardinalDirection.South), startingPhotons);
            Assert.Contains(new PhotonState(1, 0, CardinalDirection.North), startingPhotons);
        }

        [Fact]
        public void TestLavaFloorStartsWithZeroEnergizedTiles()
        {
            LavaFloor lavaFloor = new(lavaContraption);
            Assert.Equal(0, lavaFloor.NumEnergizedTiles());
        }

        [Fact]
        public void TestBeamEnergizesTilesAsItTravels()
        {
            string simpleContraption = @"..
                                         ..
                                         ..";

            LavaContraption simpleLavaContraption = new(simpleContraption);
            LavaFloor lavaFloor = new(simpleLavaContraption);
            PhotonState initialBeam = new(0, 0, CardinalDirection.East);
            lavaFloor.RunBeam(initialBeam);
            Assert.Equal(2, lavaFloor.NumEnergizedTiles());

            initialBeam = new PhotonState(1, 0, CardinalDirection.South);
            lavaFloor.RunBeam(initialBeam);
            Assert.Equal(3, lavaFloor.NumEnergizedTiles());
        }

        [Fact]
        public void TestTilesEnergizedAnMultipleTimesStillCountAsOne()
        {
            LavaFloor lavaFloor = new(lavaContraption);
            PhotonState initialBeam = new(0, 0, CardinalDirection.East);
            lavaFloor.RunBeam(initialBeam);
            Assert.Equal(46, lavaFloor.NumEnergizedTiles());
        }

        [Fact]
        public void TestCanFindInitialPhotonWhichMaximizesEnergizedTiles()
        {
            LavaFloor lavaFloor = new(lavaContraption);
            Assert.Equal(51, lavaFloor.MaxNumEnergizedTiles());
        }
    }
}