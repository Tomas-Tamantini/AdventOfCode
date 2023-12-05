namespace AdventOfCode.Tests
{
    public class TestDay5Fertilizer
    {
        private static SourceDestinationMapper BuildSourceDestinationMapper(List<RangeMapper> rangeMappers, string sourceName = "A", string destinationName = "B")
        {
            return new SourceDestinationMapper(sourceName, destinationName, rangeMappers);
        }

        private static ChainMapper BuildChainMapper()
        {
            var rangeAB = new RangeMapper(sourceStart: 0, destinationStart: 100, rangeLength: 10);
            var mapperAB = new SourceDestinationMapper(sourceName: "A", destinationName: "B", rangeMappers: new List<RangeMapper> { rangeAB });

            var rangeBC = new RangeMapper(sourceStart: 100, destinationStart: 1000, rangeLength: 10);
            var mapperBC = new SourceDestinationMapper(sourceName: "B", destinationName: "C", rangeMappers: new List<RangeMapper> { rangeBC });

            var rangeCD = new RangeMapper(sourceStart: 1000, destinationStart: 10000, rangeLength: 10);
            var mapperCD = new SourceDestinationMapper(sourceName: "C", destinationName: "D", rangeMappers: new List<RangeMapper> { rangeCD });

            return new ChainMapper(new List<SourceDestinationMapper> { mapperCD, mapperAB, mapperBC });
        }

        [Fact]
        public void TestRangeMapperReturnsNullIfSourceIsOutsideRange()
        {
            var mapper = new RangeMapper(sourceStart: 100, destinationStart: 200, rangeLength: 50);
            Assert.Null(mapper.Map(150));
            Assert.Null(mapper.Map(99));
            Assert.Null(mapper.Map(500000000000UL));
        }

        [Fact]
        public void TestRangeMapperReturnsCorrespondingDestinationIfSourceIsWithinRange()
        {
            var mapper = new RangeMapper(sourceStart: 100, destinationStart: 200, rangeLength: 50);
            Assert.Equal(200UL, mapper.Map(100));
            Assert.Equal(249UL, mapper.Map(149));
        }

        [Fact]
        public void TestSourceDestinationMapperMapsNumberToItselfIfNotWithinAnyRange()
        {
            var rangeA = new RangeMapper(sourceStart: 100, destinationStart: 200, rangeLength: 50);
            var rangeB = new RangeMapper(sourceStart: 200, destinationStart: 13, rangeLength: 14);
            var ranges = new List<RangeMapper> { rangeA, rangeB };

            var mapper = BuildSourceDestinationMapper(ranges);

            Assert.Equal(150UL, mapper.Map(150));
            Assert.Equal(99UL, mapper.Map(99));
            Assert.Equal(500000000000UL, mapper.Map(500000000000UL));
        }

        [Fact]
        public void TestSourceDestinationMapperMapsNumberWithinSomeRangeAccordingToThatRange()
        {
            var rangeA = new RangeMapper(sourceStart: 100, destinationStart: 200, rangeLength: 50);
            var rangeB = new RangeMapper(sourceStart: 200, destinationStart: 13, rangeLength: 14);
            var ranges = new List<RangeMapper> { rangeA, rangeB };

            var mapper = BuildSourceDestinationMapper(ranges);

            Assert.Equal(200UL, mapper.Map(100));
            Assert.Equal(249UL, mapper.Map(149));

            Assert.Equal(13UL, mapper.Map(200));
            Assert.Equal(26UL, mapper.Map(213));
        }

        [Fact]
        public void TestChainMapperCanChainBetweenMultipleMaps()
        {
            var chainMapper = BuildChainMapper();
            Assert.Equal(10000UL, chainMapper.Map("A", "D", 0));
        }

        [Fact]
        public void TestChainMapperMapsNumberToItselfIfSourceAndDestinationAreTheSame()
        {
            var chainMapper = BuildChainMapper();
            Assert.Equal(10000UL, chainMapper.Map("Z", "Z", 10000UL));
        }


        [Fact]
        public void TestChainMapperRaisesArgumentExceptionIfNoPathBetweenSourceAndDestination()
        {
            var chainMapper = BuildChainMapper();
            Assert.Throws<ArgumentException>(() => chainMapper.Map("B", "A", 0));
            Assert.Throws<ArgumentException>(() => chainMapper.Map("X", "Y", 0));
            Assert.Throws<ArgumentException>(() => chainMapper.Map("C", "W", 0));
        }
    }
}