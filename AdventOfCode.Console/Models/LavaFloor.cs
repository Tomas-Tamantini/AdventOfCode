namespace AdventOfCode.Console.Models
{
    public record PhotonState(int X, int Y, CardinalDirection Direction);

    enum ContraptionTile
    {
        Empty = '.',
        HorizontalSplitter = '-',
        VerticalSplitter = '|',
        AscendingMirror = '/',
        DescendingMirror = '\\',
    }

    public class LavaContraption
    {
        private readonly ContraptionTile[,] _tiles;
        private readonly int _width;
        private readonly int _height;

        public LavaContraption(string input)
        {
            string[] lines = input.Split('\n');
            _width = lines[0].Trim().Length;
            _height = lines.Length;
            _tiles = new ContraptionTile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                string line = lines[y].Trim();
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = (ContraptionTile)line[x];
                }
            }
        }

        private bool IsOutOfBounds(int x, int y) => x < 0 || x >= _width || y < 0 || y >= _height;

        private static CardinalDirection NextDirectionForEmptyTile(CardinalDirection currentDirection) => currentDirection;
        private static CardinalDirection NextDirectionForAscendingMirror(CardinalDirection currentDirection)
        {
            return currentDirection switch
            {
                CardinalDirection.North => CardinalDirection.East,
                CardinalDirection.East => CardinalDirection.North,
                CardinalDirection.South => CardinalDirection.West,
                _ => CardinalDirection.South,
            };
        }

        private static CardinalDirection NextDirectionForDescendingMirror(CardinalDirection currentDirection)
        {
            return currentDirection switch
            {
                CardinalDirection.North => CardinalDirection.West,
                CardinalDirection.East => CardinalDirection.South,
                CardinalDirection.South => CardinalDirection.East,
                _ => CardinalDirection.North,
            };
        }
        private static IEnumerable<CardinalDirection> NextDirectionsForVerticalSplitter(CardinalDirection currentDirection)
        {
            if (currentDirection == CardinalDirection.East || currentDirection == CardinalDirection.West)
            {
                yield return CardinalDirection.South;
                yield return CardinalDirection.North;
            }
            else yield return currentDirection;
        }

        private static IEnumerable<CardinalDirection> NextDirectionsForHorizontalSplitter(CardinalDirection currentDirection)
        {
            if (currentDirection == CardinalDirection.North || currentDirection == CardinalDirection.South)
            {
                yield return CardinalDirection.East;
                yield return CardinalDirection.West;
            }
            else yield return currentDirection;
        }

        private static IEnumerable<CardinalDirection> NextDirections(CardinalDirection currentDirection, ContraptionTile currentTile)
        {
            switch (currentTile)
            {
                case ContraptionTile.Empty:
                    yield return NextDirectionForEmptyTile(currentDirection);
                    break;
                case ContraptionTile.AscendingMirror:
                    yield return NextDirectionForAscendingMirror(currentDirection);
                    break;
                case ContraptionTile.DescendingMirror:
                    yield return NextDirectionForDescendingMirror(currentDirection);
                    break;
                case ContraptionTile.VerticalSplitter:
                    foreach (var nextDirection in NextDirectionsForVerticalSplitter(currentDirection))
                        yield return nextDirection;
                    break;
                case ContraptionTile.HorizontalSplitter:
                    foreach (var nextDirection in NextDirectionsForHorizontalSplitter(currentDirection))
                        yield return nextDirection;
                    break;
            }
        }
        public IEnumerable<PhotonState> NextPhotons(PhotonState currentPhoton)
        {
            foreach (var nextDirection in NextDirections(currentPhoton.Direction, _tiles[currentPhoton.X, currentPhoton.Y]))
            {
                int nextX = currentPhoton.X;
                int nextY = currentPhoton.Y;
                switch (nextDirection)
                {
                    case CardinalDirection.North:
                        nextY--;
                        break;
                    case CardinalDirection.East:
                        nextX++;
                        break;
                    case CardinalDirection.South:
                        nextY++;
                        break;
                    case CardinalDirection.West:
                        nextX--;
                        break;
                }
                if (!IsOutOfBounds(nextX, nextY))
                    yield return new PhotonState(nextX, nextY, nextDirection);
            }
        }

        public IEnumerable<PhotonState> StartingPhotons()
        {
            for (int x = 0; x < _width; x++)
            {
                yield return new PhotonState(x, 0, CardinalDirection.South);
                yield return new PhotonState(x, _height - 1, CardinalDirection.North);
            }
            for (int y = 0; y < _height; y++)
            {
                yield return new PhotonState(0, y, CardinalDirection.East);
                yield return new PhotonState(_width - 1, y, CardinalDirection.West);
            }
        }
    }
    public class LavaFloor
    {
        private readonly LavaContraption lavaContraption;
        private HashSet<(int, int)> energizedTiles = new();
        public LavaFloor(LavaContraption lavaContraption)
        {
            this.lavaContraption = lavaContraption;
        }

        public int NumEnergizedTiles()
        {
            return energizedTiles.Count;
        }

        public void RunBeam(PhotonState initialPhoton)
        {
            HashSet<PhotonState> visitedPhotons = new();
            List<PhotonState> photonsToVisitStack = new() { initialPhoton };
            while (photonsToVisitStack.Any())
            {
                PhotonState currentPhoton = photonsToVisitStack.Last();
                photonsToVisitStack.RemoveAt(photonsToVisitStack.Count - 1);
                if (visitedPhotons.Contains(currentPhoton))
                    continue;
                visitedPhotons.Add(currentPhoton);
                foreach (var nextPhoton in lavaContraption.NextPhotons(currentPhoton))
                {
                    photonsToVisitStack.Add(nextPhoton);
                }
            }
            energizedTiles = visitedPhotons.Select(p => (p.X, p.Y)).ToHashSet();
        }

        public int MaxNumEnergizedTiles()
        {
            // TODO: Make more efficient (maybe memoization?)
            int maxNumEnergizedTiles = 0;
            foreach (var initialPhoton in lavaContraption.StartingPhotons())
            {
                RunBeam(initialPhoton);
                maxNumEnergizedTiles = Math.Max(maxNumEnergizedTiles, NumEnergizedTiles());
            }
            return maxNumEnergizedTiles;
        }

    }
}