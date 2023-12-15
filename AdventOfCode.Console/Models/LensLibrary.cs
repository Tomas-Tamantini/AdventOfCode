namespace AdventOfCode.Console.Models
{
    public class Lens
    {
        public string Label { get; set; } = string.Empty;
        public int FocalLenght { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj is Lens other)
            {
                return Label == other.Label && FocalLenght == other.FocalLenght;
            }
            return false;
        }
    }

    public class LensLibrary
    {
        public LinkedList<Lens>[] Boxes { get; }

        public LensLibrary()
        {
            Boxes = new LinkedList<Lens>[256];
            for (int i = 0; i < 256; i++)
            {
                Boxes[i] = new LinkedList<Lens>();
            }
        }

        public void Upsert(Lens lens)
        {
            int hash = GetHash(lens.Label);
            foreach (var existingLens in Boxes[hash])
            {
                if (existingLens.Label == lens.Label)
                {
                    existingLens.FocalLenght = lens.FocalLenght;
                    return;
                }
            }
            Boxes[hash].AddLast(lens);
        }

        public void Remove(string lensLabel)
        {
            int hash = GetHash(lensLabel);
            foreach (var existingLens in Boxes[hash])
            {
                if (existingLens.Label == lensLabel)
                {
                    Boxes[hash].Remove(existingLens);
                    return;
                }
            }
        }

        public int BoxFocusingPower(int boxNumber)
        {
            return Boxes[boxNumber].Select((lens, i) => lens.FocalLenght * (i + 1)).Sum();
        }

        public static int GetHash(string s)
        {
            int hash = 0;
            foreach (char c in s)
            {
                hash += c;
                hash = hash * 17 % 256;
            }
            return hash;
        }
    }
}