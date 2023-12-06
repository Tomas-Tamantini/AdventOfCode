using Moq;

namespace AdventOfCode.Tests
{
    public class TestDay5Fertilizer
    {
        #region Fertilizer tests

        private Mock<IIntervalSet> outputIntervalMock;
        private Mock<IIntervalMapper> intervalMapperMock;
        private Fertilizer fertilizer;

        private void SetupFertilizerMocks()
        {
            outputIntervalMock = new Mock<IIntervalSet>();
            outputIntervalMock.Setup(x => x.LowestNumber()).Returns(123);

            intervalMapperMock = new Mock<IIntervalMapper>();
            intervalMapperMock.Setup(x => x.Map(It.IsAny<IIntervalSet>())).Returns(outputIntervalMock.Object);

            fertilizer = new Fertilizer(new List<long> { 10, 3 }, intervalMapperMock.Object);
        }


        [Fact]
        public void TestLowestOutputWithStandaloneSeedsReturnsSmallestNumberOfOutputSet()
        {
            SetupFertilizerMocks();
            Assert.Equal(123, fertilizer.LowestOutputWithStandaloneSeeds());
        }

        [Fact]
        public void TestStandaloneSeedsArePassedAsIndividualIntervalsToIntervalMapper()
        {
            SetupFertilizerMocks();
            _ = fertilizer.LowestOutputWithStandaloneSeeds();
            intervalMapperMock.Verify(x => x.Map(It.Is<IIntervalSet>(y => y.Contains(3) && y.Contains(10) && !y.Contains(7))));
        }

        [Fact]
        public void TestLowestOutputWithSeedsAsRangesReturnsSmallestNumberOfOutputSet()
        {
            SetupFertilizerMocks();
            Assert.Equal(123, fertilizer.LowestOutputWithSeedsAsRanges());
        }

        [Fact]
        public void TestSeedsAsRangesArePassedAsSingleIntervalToIntervalMapper()
        {
            SetupFertilizerMocks();
            _ = fertilizer.LowestOutputWithSeedsAsRanges();
            intervalMapperMock.Verify(x => x.Map(It.Is<IIntervalSet>(y => y.Contains(10) && y.Contains(11) && y.Contains(12) && !y.Contains(13))));
        }

        #endregion

        #region Interval tests

        [Fact]
        public void TestIntervalSetInformsWhetherItCointainsNumber()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 } });

            Assert.True(intervalSet.Contains(1));
            Assert.True(intervalSet.Contains(2));
            Assert.True(intervalSet.Contains(3));
            Assert.False(intervalSet.Contains(4));
        }

        [Fact]
        public void TestIntervalSetInformsWhetherItContainsAllInAGroupOfNumbers()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 } });

            Assert.True(intervalSet.ContainsAll(1, 2, 3));
            Assert.False(intervalSet.ContainsAll(1, 2, 3, 4));
        }

        [Fact]
        public void TestIntervalSetInformsWhetherItContainsAnyInAGroupOfNumbers()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 } });

            Assert.True(intervalSet.ContainsAny(1, 2, 3));
            Assert.True(intervalSet.ContainsAny(1, 2, 3, 4));
            Assert.False(intervalSet.ContainsAny(4, 5, 6));
        }

        [Fact]
        public void TestSlicingAnAreaWhereSetIsEmptyReturnsEmptySet()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 } });
            var slicedSet = intervalSet.Slice(4, 5);
            Assert.True(slicedSet.IsEmpty());
        }

        [Fact]
        public void TestIntervalSetInformsItsLowestNumber()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 }, new() { Start = 10, End = 14 } });
            Assert.Equal(1, intervalSet.LowestNumber());
        }

        [Fact]
        public void TestSlicingAnAreaWhereSetIsNotEmptyReturnsSetWithSlicedIntervals()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 }, new() { Start = 10, End = 14 } });
            var slicedSet = intervalSet.Slice(2, 5);
            Assert.True(slicedSet.ContainsAll(2, 3));
            Assert.False(slicedSet.ContainsAny(1, 4));
        }

        [Fact]
        public void TestCanPassOnlySliceStart()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 }, new() { Start = 10, End = 14 } });
            var slicedSet = intervalSet.Slice(start: 3);
            Assert.True(slicedSet.ContainsAll(3));
            Assert.False(slicedSet.ContainsAny(1, 2, 4));
        }

        [Fact]
        public void TestCanPassOnlySliceEnd()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 }, new() { Start = 10, End = 14 } });
            var slicedSet = intervalSet.Slice(end: 3);
            Assert.True(slicedSet.ContainsAll(1, 2, 3));
            Assert.False(slicedSet.ContainsAny(0, 4));
        }

        [Fact]
        public void TestMergingTwoSetsProducesSetWithIntervalsFromBothSets()
        {
            var intervalSet1 = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 } });
            var intervalSet2 = new IntervalSet(new List<Interval> { new() { Start = 3, End = 5 }, new() { Start = 10, End = 14 } });
            var mergedSet = intervalSet1.Merge(intervalSet2);
            Assert.True(mergedSet.ContainsAll(1, 2, 3, 4, 5, 10, 11, 12, 13, 14));
        }

        [Fact]
        public void TestCanOffsetSet()
        {
            var intervalSet = new IntervalSet(new List<Interval> { new() { Start = 1, End = 3 }, new() { Start = 10, End = 14 } });
            var offsetSet = intervalSet.Offset(100);
            Assert.True(offsetSet.ContainsAll(101, 102, 103, 110, 111, 112, 113, 114));
        }

        # endregion

        #region ChainMapper tests

        [Fact]
        public void TestChainMapperMapsAppliesConsecutiveMappings()
        {
            var intervalSetMockIn = new Mock<IIntervalSet>();
            var intervalSetMockOut1 = new Mock<IIntervalSet>();
            var intervalSetMockOut2 = new Mock<IIntervalSet>();

            var mapperMock1 = new Mock<IIntervalMapper>();
            var mapperMock2 = new Mock<IIntervalMapper>();

            mapperMock1.Setup(x => x.Map(It.IsAny<IIntervalSet>())).Returns(intervalSetMockOut1.Object);
            mapperMock2.Setup(x => x.Map(It.IsAny<IIntervalSet>())).Returns(intervalSetMockOut2.Object);

            var chainMapper = new ChainMapper(new List<IIntervalMapper> { mapperMock1.Object, mapperMock2.Object });
            var result = chainMapper.Map(intervalSetMockIn.Object);

            mapperMock1.Verify(x => x.Map(intervalSetMockIn.Object));
            mapperMock2.Verify(x => x.Map(intervalSetMockOut1.Object));
            Assert.Equal(intervalSetMockOut2.Object, result);
        }

        #endregion

        #region SourceDestination tests

        [Fact]
        public void TestSourceDestinationMapperMapsEmptyIntervalSetToEmpty()
        {
            var emptyIntervalSet = new IntervalSet(new List<Interval>());
            var intervalOffset = new IntervalOffset(new Interval { Start = 1, End = 3 }, 2);
            var intervalOffsets = new List<IntervalOffset> { intervalOffset };
            var sourceDestinationMapper = new SourceDestinationMapper("A", "B", intervalOffsets);
            var outputInterval = sourceDestinationMapper.Map(emptyIntervalSet);
            Assert.True(outputInterval.IsEmpty());
        }

        [Fact]
        public void TestSourceDestinationMapperMapsIntervalSetToItselfIfOffsetsDoNotIntersectInputSet()
        {
            IntervalSet inputSet = new(new List<Interval>() { new() { Start = 1, End = 3 } });
            var intervalOffsets = new List<IntervalOffset> { new(new Interval { Start = 4, End = 5 }, 2) };
            var sourceDestinationMapper = new SourceDestinationMapper("A", "B", intervalOffsets);
            var outputSet = sourceDestinationMapper.Map(inputSet);
            Assert.True(outputSet.ContainsAll(1, 2, 3));
        }

        [Fact]
        public void TestSourceDestinationMapperSlicesAndOffsetsInputSetToProduceOutput()
        {
            IntervalSet inputSet = new(new List<Interval>() { new() { Start = 1, End = 3 }, new() { Start = 10, End = 14 } });
            IntervalOffset offset1 = new(new Interval { Start = 2, End = 11 }, 100);
            IntervalOffset offset2 = new(new Interval { Start = 14, End = 20 }, 1000);
            var sourceDestinationMapper = new SourceDestinationMapper("A", "B", new List<IntervalOffset> { offset1, offset2 });
            var outputSet = sourceDestinationMapper.Map(inputSet);
            Assert.True(outputSet.ContainsAll(1, 12, 13, 102, 103, 110, 111, 1014));
            Assert.False(outputSet.ContainsAny(2, 3, 10, 11, 14));
        }

        #endregion
    }
}