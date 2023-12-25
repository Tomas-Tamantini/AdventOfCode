namespace AdventOfCode.Tests
{
    public class TestDay25Snowverload
    {
        [Fact]
        public void TestCannotSplitGraphWithSingleNode()
        {
            HashSet<string> nodes = new() { "A" };
            WeightedUndirectedGraph graph = new(nodes);
            Assert.Throws<InvalidOperationException>(() => Snowverload.MinCut(graph));
        }

        [Fact]
        public void TestCanSplitGraphWithTwoNodes()
        {
            HashSet<string> nodes = new() { "A", "B" };
            WeightedUndirectedGraph graph = new(nodes);
            graph.AddEdge("A", "B");
            (HashSet<string> firstGroup, HashSet<string> secondGroup) = Snowverload.MinCut(graph);
            if (firstGroup.Contains("A"))
            {
                Assert.Equal(new HashSet<string> { "A" }, firstGroup);
                Assert.Equal(new HashSet<string> { "B" }, secondGroup);
            }
            else
            {
                Assert.Equal(new HashSet<string> { "B" }, firstGroup);
                Assert.Equal(new HashSet<string> { "A" }, secondGroup);
            }
        }

        [Fact]
        public void TestGraphIsSplitWithMinimumNumberOfCuts()
        {
            HashSet<string> nodes = new() { "A", "B", "C", "D" };
            WeightedUndirectedGraph graph = new(nodes);
            graph.AddEdge("A", "B");
            graph.AddEdge("A", "C");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "D");
            (HashSet<string> firstGroup, HashSet<string> secondGroup) = Snowverload.MinCut(graph);
            if (firstGroup.Contains("A"))
            {
                Assert.Equal(new HashSet<string> { "A", "B", "C" }, firstGroup);
                Assert.Equal(new HashSet<string> { "D" }, secondGroup);
            }
            else
            {
                Assert.Equal(new HashSet<string> { "D" }, firstGroup);
                Assert.Equal(new HashSet<string> { "A", "B", "C" }, secondGroup);
            }
        }


        [Fact]
        public void TestGraphIsSplitWithMinimumNumberOfCutsEfficiently()
        {
            HashSet<string> nodes = new() { "A1", "B1", "C1", "D1", "E1", "A2", "B2", "C2", "D2", "E2" };
            WeightedUndirectedGraph graph = new(nodes);
            graph.AddEdge("A1", "B1");
            graph.AddEdge("A1", "C1");
            graph.AddEdge("A1", "D1");
            graph.AddEdge("A1", "E1");
            graph.AddEdge("B1", "C1");
            graph.AddEdge("B1", "D1");
            graph.AddEdge("B1", "E1");
            graph.AddEdge("C1", "D1");
            graph.AddEdge("C1", "E1");
            graph.AddEdge("D1", "E1");

            graph.AddEdge("A2", "B2");
            graph.AddEdge("A2", "C2");
            graph.AddEdge("A2", "D2");
            graph.AddEdge("A2", "E2");
            graph.AddEdge("B2", "C2");
            graph.AddEdge("B2", "D2");
            graph.AddEdge("B2", "E2");
            graph.AddEdge("C2", "D2");
            graph.AddEdge("C2", "E2");
            graph.AddEdge("D2", "E2");

            graph.AddEdge("C1", "C2");
            graph.AddEdge("C1", "D2");
            graph.AddEdge("E1", "B2");

            (HashSet<string> firstGroup, HashSet<string> secondGroup) = Snowverload.MinCut(graph);
            if (firstGroup.Contains("A1"))
            {
                Assert.Equal(new HashSet<string> { "A1", "B1", "C1", "D1", "E1" }, firstGroup);
                Assert.Equal(new HashSet<string> { "A2", "B2", "C2", "D2", "E2" }, secondGroup);
            }
            else
            {
                Assert.Equal(new HashSet<string> { "A2", "B2", "C2", "D2", "E2" }, firstGroup);
                Assert.Equal(new HashSet<string> { "A1", "B1", "C1", "D1", "E1" }, secondGroup);
            }
        }
    }
}