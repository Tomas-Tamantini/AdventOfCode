namespace AdventOfCode.Console.Models
{
    enum HikeTile
    {
        Path = '.',
        Forest = '#',
        SlopeNorth = '^',
        SlopeSouth = 'v',
        SlopeEast = '>',
        SlopeWest = '<'
    }

    public class Forest
    {
        private readonly HikeTile[,] _tiles;
        private readonly int _width;
        private readonly int _height;
        private readonly (int, int) _startPosition;
        private readonly (int, int) _endPosition;

        public (int, int) StartPosition => _startPosition;
        public (int, int) EndPosition => _endPosition;

        public Forest(string input)
        {
            string[] lines = input.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _tiles = new HikeTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = (HikeTile)line[x];
                    if (_tiles[x, y] == HikeTile.Path)
                    {
                        if (y == 0) { _startPosition = (x, y); }
                        if (y == _height - 1) { _endPosition = (x, y); }
                    }
                }
            }
        }

        private static IEnumerable<(int, int)> NeighboringCoordinates((int, int) currentCoordinates)
        {
            (int x, int y) = currentCoordinates;
            yield return (x + 1, y);
            yield return (x - 1, y);
            yield return (x, y + 1);
            yield return (x, y - 1);
        }

        private bool IsOutOfBounds((int, int) coordinates)
        {
            (int x, int y) = coordinates;
            return x < 0 || y < 0 || x >= _width || y >= _height;
        }

        private bool ViolatesSlope((int, int) currentPosition, (int, int) nextPosition)
        {
            var currentTile = _tiles[currentPosition.Item1, currentPosition.Item2];
            int dx = nextPosition.Item1 - currentPosition.Item1;
            int dy = nextPosition.Item2 - currentPosition.Item2;
            return currentTile switch
            {
                HikeTile.SlopeEast => dx != 1,
                HikeTile.SlopeWest => dx != -1,
                HikeTile.SlopeSouth => dy != 1,
                HikeTile.SlopeNorth => dy != -1,
                _ => false,
            };
        }

        public IEnumerable<(int, int)> ValidNeighbors((int, int) currentPosition, bool considerSlopes = true)
        {
            foreach (var neighbor in NeighboringCoordinates(currentPosition))
            {
                if (IsOutOfBounds(neighbor) || _tiles[neighbor.Item1, neighbor.Item2] == HikeTile.Forest) continue;
                if (considerSlopes && ViolatesSlope(currentPosition, neighbor)) continue;
                yield return neighbor;
            }
        }
    }

    class WeightedGraph
    {
        private readonly Dictionary<(int, int), Dictionary<(int, int), int>> _edges = new();
        public void AddEdge((int, int) node1, (int, int) node2, int weight)
        {
            if (!_edges.ContainsKey(node1)) _edges[node1] = new();
            if (!_edges.ContainsKey(node2)) _edges[node2] = new();
            _edges[node1][node2] = weight;
            _edges[node2][node1] = weight;
        }

        private int Weight((int, int) node1, (int, int) node2)
        {
            if (!_edges.ContainsKey(node1) || !_edges[node1].ContainsKey(node2)) return -1;
            return _edges[node1][node2];
        }

        public void ReduceCorridors()
        {
            HashSet<(int, int)> nodesToRemove = new();
            foreach (var node in _edges.Keys)
            {
                var neighbors = Neighbors(node).ToList();
                if (neighbors.Count != 2) continue;
                var neighbor1 = neighbors[0];
                var neighbor2 = neighbors[1];
                int newWeight = _edges[node][neighbor1] + _edges[node][neighbor2];
                int previousWeight = Weight(neighbor1, neighbor2);
                int weight = Math.Max(newWeight, previousWeight);
                _edges[neighbor1][neighbor2] = weight;
                _edges[neighbor2][neighbor1] = weight;
                nodesToRemove.Add(node);
                _edges[neighbor1].Remove(node);
                _edges[neighbor2].Remove(node);
            }
            foreach (var node in nodesToRemove)
            {
                _edges.Remove(node);
            }
        }

        public int LengthLongestPath((int, int) startNode, (int, int) endNode)
        {
            HashSet<(int, int)> visitedNodes = new();
            int numSteps = NumStepsRecursive(startNode, endNode, visitedNodes);
            return numSteps >= 0 ? numSteps + 1 : -1;
        }

        private IEnumerable<(int, int)> Neighbors((int, int) node)
        {
            if (!_edges.ContainsKey(node)) yield break;
            foreach (var neighbor in _edges[node].Keys)
            {
                yield return neighbor;
            }
        }

        private int NumStepsRecursive((int, int) currentNode, (int, int) endNode, HashSet<(int, int)> visitedNodes)
        {
            if (currentNode == endNode) return 0;
            visitedNodes.Add(currentNode);
            int maxNumSteps = -1;
            foreach (var neighbor in Neighbors(currentNode))
            {
                if (visitedNodes.Contains(neighbor)) continue;
                int numSteps = NumStepsRecursive(neighbor, endNode, visitedNodes);
                if (numSteps >= 0) numSteps += Weight(currentNode, neighbor);
                maxNumSteps = Math.Max(maxNumSteps, numSteps);
            }
            visitedNodes.Remove(currentNode);
            return maxNumSteps >= 0 ? maxNumSteps : -1;
        }
    }

    public class LongWalk
    {
        private readonly Forest _forest;
        public LongWalk(Forest forest)
        {
            _forest = forest;
        }

        public int LengthLongestPath()
        {
            // TODO: Remove duplication - use WeightedGraph class
            HashSet<(int, int)> visitedTiles = new();
            return LengthLongestPathRecursive(_forest.StartPosition, visitedTiles);
        }

        public int LengthLongestPathIgnoringSlopes()
        {
            WeightedGraph graph = new();
            List<(int, int)> nodesToAdd = new() { _forest.StartPosition };
            HashSet<(int, int)> visitedNodes = new();
            while (nodesToAdd.Any())
            {
                var node = nodesToAdd.Last();
                nodesToAdd.RemoveAt(nodesToAdd.Count - 1);
                if (visitedNodes.Contains(node)) continue;
                visitedNodes.Add(node);
                foreach (var neighbor in _forest.ValidNeighbors(node, considerSlopes: false))
                {
                    graph.AddEdge(node, neighbor, weight: 1);
                    nodesToAdd.Add(neighbor);
                }
            }
            graph.ReduceCorridors();
            return graph.LengthLongestPath(_forest.StartPosition, _forest.EndPosition);

        }

        private int LengthLongestPathRecursive((int, int) currentPosition, HashSet<(int, int)> visitedPositions)
        {
            if (currentPosition == _forest.EndPosition) return 1;
            visitedPositions.Add(currentPosition);
            int maxLength = -1;
            foreach (var neighbor in _forest.ValidNeighbors(currentPosition))
            {
                if (visitedPositions.Contains(neighbor)) continue;
                int pathLength = LengthLongestPathRecursive(neighbor, visitedPositions);
                maxLength = Math.Max(maxLength, pathLength);
            }
            visitedPositions.Remove(currentPosition);
            return maxLength >= 0 ? maxLength + 1 : -1;
        }
    }
}