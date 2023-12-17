namespace AdventOfCode.Console.Models
{
    record CrucibleState(int X, int Y, CardinalDirection? Direction, int NumStepsSameDirection);

    public record CrucibleInertia(int? MinStepsSameDirection, int? MaxStepsSameDirection);

    public class ClumsyCrucible
    {
        private readonly int[,] cityBlocks;
        private readonly int width;
        private readonly int height;
        private readonly CrucibleInertia inertia;
        public ClumsyCrucible(string cityBlocksStr) : this(cityBlocksStr, new CrucibleInertia(MinStepsSameDirection: null, MaxStepsSameDirection: null)) { }

        public ClumsyCrucible(string cityBlocksStr, CrucibleInertia crucibleInertia)
        {
            inertia = crucibleInertia;
            string[] cityBlocksRows = cityBlocksStr.Split(Environment.NewLine);
            width = cityBlocksRows[0].Trim().Length;
            height = cityBlocksRows.Length;
            cityBlocks = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                string row = cityBlocksRows[y].Trim();
                for (int x = 0; x < width; x++)
                {
                    cityBlocks[x, y] = int.Parse(row[x].ToString());
                }
            }
        }

        private static bool IsDestination(CrucibleState state, (int x, int y) destination)
        {
            return state.X == destination.x && state.Y == destination.y;
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private static bool DirectionIsReversed(CardinalDirection direction, CardinalDirection? previousDirection)
        {
            if (previousDirection == null) return false;
            return direction switch
            {
                CardinalDirection.North => previousDirection == CardinalDirection.South,
                CardinalDirection.East => previousDirection == CardinalDirection.West,
                CardinalDirection.South => previousDirection == CardinalDirection.North,
                _ => previousDirection == CardinalDirection.East,
            };
        }

        private static int NumStepsSameDirection(CrucibleState currentState, CardinalDirection newDirection)
        {
            if (currentState.Direction == newDirection)
            {
                return currentState.NumStepsSameDirection + 1;
            }
            return 1;
        }

        private bool DirectionIsValid(CardinalDirection direction, CrucibleState currentState)
        {
            if (DirectionIsReversed(direction, currentState.Direction))
            {
                return false;
            }
            int numStepsSameDirection = NumStepsSameDirection(currentState, direction);
            if (currentState.Direction == direction)
            {
                if (inertia.MaxStepsSameDirection != null && numStepsSameDirection > inertia.MaxStepsSameDirection)
                {
                    return false;
                }
            }
            else if (currentState.Direction != null && inertia.MinStepsSameDirection != null && currentState.NumStepsSameDirection < inertia.MinStepsSameDirection)
            {
                return false;
            }
            return true;
        }

        private IEnumerable<CardinalDirection> ValidDirectionsAndNumSteps(CrucibleState currentState)
        {
            foreach (CardinalDirection direction in Enum.GetValues(typeof(CardinalDirection)))
            {
                if (DirectionIsValid(direction, currentState))
                {
                    yield return direction;
                }

            }
        }


        private IEnumerable<CrucibleState> GetNeighbors(CrucibleState currentState)
        {
            foreach (var direction in ValidDirectionsAndNumSteps(currentState))
            {
                int newX = currentState.X;
                int newY = currentState.Y;
                switch (direction)
                {
                    case CardinalDirection.North:
                        newY--;
                        break;
                    case CardinalDirection.East:
                        newX++;
                        break;
                    case CardinalDirection.South:
                        newY++;
                        break;
                    case CardinalDirection.West:
                        newX--;
                        break;
                }
                if (IsInBounds(newX, newY))
                {
                    int numStepsSameDirection = NumStepsSameDirection(currentState, direction);
                    yield return new CrucibleState(newX, newY, direction, numStepsSameDirection);
                }
            }
        }

        public int MinimumHeatLoss()
        {
            return MinimumHeatLoss(start: (0, 0), destination: (width - 1, height - 1));
        }

        public int MinimumHeatLoss((int x, int y) start, (int x, int y) destination)
        {
            // Dijkstra's algorithm
            CrucibleState intialNode = new(start.x, start.y, Direction: null, NumStepsSameDirection: 0);
            Dictionary<CrucibleState, int> distances = new()
            {
                { intialNode, 0 }
            };
            HashSet<CrucibleState> visited = new();
            PriorityQueue<CrucibleState, int> queue = new();
            queue.Enqueue(intialNode, 0);

            while (queue.Count > 0)
            {
                CrucibleState current = queue.Dequeue();
                if (IsDestination(current, destination))
                {
                    return distances[current];
                }
                if (visited.Contains(current))
                {
                    continue;
                }
                visited.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    int distance = distances[current] + cityBlocks[neighbor.X, neighbor.Y];
                    if (!distances.ContainsKey(neighbor) || distance < distances[neighbor])
                    {
                        distances[neighbor] = distance;
                        queue.Enqueue(neighbor, distance);
                    }
                }
            }
            return -1;
        }
    }
}