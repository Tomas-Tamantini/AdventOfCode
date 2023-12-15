namespace AdventOfCode.Console.Models
{
    public static class LensLibrary
    {
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