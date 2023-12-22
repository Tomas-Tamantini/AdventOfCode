namespace AdventOfCode.Console.Models
{
    public record Coordinates(int X, int Y, int Z);

    public class SandBrick
    {
        private string id;
        private Coordinates bottomCubeCoordinates;
        private readonly int width;
        private readonly int height;
        private readonly int depth;

        public SandBrick(
            string id,
            Coordinates bottomCubeCoordinates,
            int width = 1,
            int height = 1,
            int depth = 1
        )
        {
            this.id = id;
            this.bottomCubeCoordinates = bottomCubeCoordinates;
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        public string Id => id;

        public void ResetZCoordinate(int z)
        {
            bottomCubeCoordinates = new Coordinates(bottomCubeCoordinates.X, bottomCubeCoordinates.Y, z);
        }

        public IEnumerable<Coordinates> BottomCoordinates()
        {
            for (int x = bottomCubeCoordinates.X; x < bottomCubeCoordinates.X + width; x++)
            {
                for (int y = bottomCubeCoordinates.Y; y < bottomCubeCoordinates.Y + depth; y++)
                {
                    yield return new Coordinates(x, y, bottomCubeCoordinates.Z);
                }
            }
        }

        public IEnumerable<Coordinates> TopCoordinates()
        {
            for (int x = bottomCubeCoordinates.X; x < bottomCubeCoordinates.X + width; x++)
            {
                for (int y = bottomCubeCoordinates.Y; y < bottomCubeCoordinates.Y + depth; y++)
                {
                    yield return new Coordinates(x, y, bottomCubeCoordinates.Z + height - 1);
                }
            }
        }
    }

    public class BrickPile
    {
        private readonly Dictionary<(int x, int y), int> heights;
        private readonly Dictionary<(int x, int y), string> brickIds;

        public BrickPile()
        {
            heights = new();
            brickIds = new();
        }

        public HashSet<string> PlaceBrickAndReturnSupportingBrickIds(SandBrick brick)
        {
            int newHeight = 0;
            HashSet<string> supportingBricksIds = new();

            foreach (Coordinates coordinates in brick.BottomCoordinates())
            {
                if (!brickIds.ContainsKey((coordinates.X, coordinates.Y)))
                {
                    continue;
                }
                int brickHeight = heights[(coordinates.X, coordinates.Y)];
                if (brickHeight < newHeight) continue;
                if (brickHeight > newHeight)
                {
                    newHeight = brickHeight;
                    supportingBricksIds.Clear();
                }
                supportingBricksIds.Add(brickIds[(coordinates.X, coordinates.Y)]);
            }

            brick.ResetZCoordinate(newHeight + 1);
            foreach (Coordinates coordinates in brick.TopCoordinates())
            {
                brickIds[(coordinates.X, coordinates.Y)] = brick.Id;
                heights[(coordinates.X, coordinates.Y)] = coordinates.Z;
            }
            return supportingBricksIds;
        }
    }

    public class SandSlabs
    {
        private readonly List<SandBrick> bricks;
        private readonly BrickPile pile;
        private readonly Dictionary<string, HashSet<string>> brickSupports;
        private readonly Dictionary<string, HashSet<string>> brickIsSupportedBy;

        public SandSlabs(IEnumerable<SandBrick> bricksSnapshot)
        {
            bricks = bricksSnapshot.ToList();
            bricks.Sort((b1, b2) => b1.BottomCoordinates().First().Z.CompareTo(b2.BottomCoordinates().First().Z));
            pile = new();
            brickSupports = new();
            brickIsSupportedBy = new();
        }

        public IEnumerable<string> BricksSupportedBy(string brickId) => brickSupports.GetValueOrDefault(brickId, new());
        public IEnumerable<string> BricksSupporting(string brickId) => brickIsSupportedBy.GetValueOrDefault(brickId, new());

        public void DropBricks()
        {
            brickSupports.Clear();
            brickIsSupportedBy.Clear();
            foreach (SandBrick brick in bricks)
            {
                HashSet<string> supportingBricksIds = pile.PlaceBrickAndReturnSupportingBrickIds(brick);
                brickIsSupportedBy[brick.Id] = supportingBricksIds;
                foreach (string supportingBrickId in supportingBricksIds)
                {
                    if (!brickSupports.ContainsKey(supportingBrickId))
                    {
                        brickSupports[supportingBrickId] = new();
                    }
                    brickSupports[supportingBrickId].Add(brick.Id);
                }
            }
        }

        private bool BrickCanBeSafelyDisintegrated(string brickId)
        {
            return BricksSupportedBy(brickId).All(supportingBrickId => BricksSupporting(supportingBrickId).Count() > 1);
        }

        public IEnumerable<string> SafeToDisintegrateBrickIds()
        {
            return bricks.Select(brick => brick.Id)
                         .Where(brickId => BrickCanBeSafelyDisintegrated(brickId));
        }

        private bool BrickIsOnTheFloor(string brickId) => !BricksSupporting(brickId).Any();

        public int CountBricksThatFallWhenDisintegrating(string brickId)
        {
            HashSet<string> fallenBricks = new() { brickId };
            foreach (string otherBrickId in bricks.Select(brick => brick.Id))
            {
                if (BrickIsOnTheFloor(otherBrickId)) continue;
                if (BricksSupporting(otherBrickId).All(supportingBrickId => fallenBricks.Contains(supportingBrickId)))
                {
                    fallenBricks.Add(otherBrickId);
                }
            }
            return fallenBricks.Count - 1;
        }
    }
}