namespace AdventOfCode.Console.Models
{
    // TODO: Refactor to reduce cognitive complexity
    public class WeightedUndirectedGraph
    {
        private readonly Dictionary<string, Dictionary<string, int>> adjacencies;

        public WeightedUndirectedGraph(HashSet<string> nodes)
        {
            adjacencies = new();
            foreach (string node in nodes)
            {
                adjacencies[node] = new Dictionary<string, int>();
            }
        }

        public void AddEdge(string nodeA, string nodeB, int weight = 1)
        {
            adjacencies[nodeA][nodeB] = weight;
            adjacencies[nodeB][nodeA] = weight;
        }

        public int NumNodes => adjacencies.Count;

        public HashSet<string> Nodes => adjacencies.Keys.ToHashSet();

        public WeightedUndirectedGraph Copy()
        {
            WeightedUndirectedGraph copy = new(new HashSet<string>(adjacencies.Keys));
            foreach (string node in adjacencies.Keys)
            {
                foreach (string neighbor in adjacencies[node].Keys)
                {
                    copy.AddEdge(node, neighbor, adjacencies[node][neighbor]);
                }
            }
            return copy;
        }

        public void RemoveNode(string node)
        {
            adjacencies.Remove(node);
            foreach (string neighbor in adjacencies.Keys)
            {
                if (adjacencies[neighbor].ContainsKey(node))
                {
                    adjacencies[neighbor].Remove(node);
                }
            }
        }

        public void AddNode(string node, Dictionary<string, int> neighbors)
        {
            adjacencies[node] = neighbors;
            foreach (string neighbor in neighbors.Keys)
            {
                AddEdge(node, neighbor, neighbors[neighbor]);
            }
        }

        public void MergeNodes(string nodeA, string nodeB, char separator)
        {
            Dictionary<string, int> mergedNeighbors = new();
            foreach (string neighbor in adjacencies[nodeA].Keys)
            {
                if (neighbor != nodeB)
                {
                    mergedNeighbors[neighbor] = adjacencies[nodeA][neighbor];
                }
            }
            foreach (string neighbor in adjacencies[nodeB].Keys)
            {
                if (neighbor != nodeA)
                {
                    if (mergedNeighbors.ContainsKey(neighbor))
                    {
                        mergedNeighbors[neighbor] += adjacencies[nodeB][neighbor];
                    }
                    else
                    {
                        mergedNeighbors[neighbor] = adjacencies[nodeB][neighbor];
                    }
                }
            }
            RemoveNode(nodeA);
            RemoveNode(nodeB);
            string newNode = nodeA + separator + nodeB;
            AddNode(newNode, mergedNeighbors);
        }

        public int CutSize(string node, HashSet<string> group)
        {
            int cutSize = 0;
            foreach (string neighbor in adjacencies[node].Keys)
            {
                if (group.Contains(neighbor))
                {
                    cutSize += adjacencies[node][neighbor];
                }
            }
            return cutSize;
        }

        public string MostConnectedNode(HashSet<string> group)
        {
            int maxDegree = 0;
            string mostConnectedNode = "";
            foreach (string node in adjacencies.Keys)
            {
                if (!group.Contains(node))
                {
                    int degree = 0;
                    foreach (string neighbor in adjacencies[node].Keys)
                    {
                        if (group.Contains(neighbor))
                        {
                            degree += adjacencies[node][neighbor];
                        }
                    }
                    if (degree > maxDegree)
                    {
                        maxDegree = degree;
                        mostConnectedNode = node;
                    }
                }
            }
            return mostConnectedNode;
        }
    }

    public class Snowverload
    {
        public static (HashSet<string>, HashSet<string>) MinCut(WeightedUndirectedGraph graph, int? knownMinCut = null)
        {
            if (graph.NumNodes <= 1) throw new InvalidOperationException();
            WeightedUndirectedGraph tempGraph = graph.Copy();
            HashSet<string> groupA = StoerWagner(tempGraph, knownMinCut);
            HashSet<string> groupB = new(graph.Nodes.Except(groupA));
            return (groupA, groupB);
        }

        private static HashSet<string> StoerWagner(WeightedUndirectedGraph graph, int? knownMinCut)
        {
            int minCut = int.MaxValue;
            string minCutGroup = "";
            char separator = '-';
            while (graph.NumNodes > 1)
            {
                (int cut, string superNodeLast, string superNodeBeforeLast) = MinCutIteration(graph);
                if (cut < minCut)
                {
                    minCut = cut;
                    minCutGroup = superNodeLast;
                    if (knownMinCut != null && minCut == knownMinCut) break;

                }
                graph.MergeNodes(superNodeLast, superNodeBeforeLast, separator);
            }
            return minCutGroup.Split(separator).ToHashSet();
        }

        private static (int, string, string) MinCutIteration(WeightedUndirectedGraph graph)
        {
            if (graph.NumNodes == 2)
            {
                string nodeA = graph.Nodes.First();
                string nodeB = graph.Nodes.Last();
                int cutSize = graph.CutSize(nodeA, new HashSet<string> { nodeB });
                return (cutSize, nodeA, nodeB);
            }
            HashSet<string> currentGroup = new() { graph.Nodes.First() };
            for (int i = 0; i < graph.NumNodes - 3; i++)
            {
                string nextNode = graph.MostConnectedNode(currentGroup);
                currentGroup.Add(nextNode);
            }
            string superNodeBeforeLast = graph.MostConnectedNode(currentGroup);
            currentGroup.Add(superNodeBeforeLast);
            string superNodeLast = graph.Nodes.Except(currentGroup).First();
            int cut = graph.CutSize(superNodeLast, currentGroup);
            return (cut, superNodeLast, superNodeBeforeLast);
        }
    }
}