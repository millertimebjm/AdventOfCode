namespace AdventOfCodeCSharp.Day03
{
    public static class PriorityService
    {
        public static int GetPriority(char c)
        {
            var intValue = (int)c;
            if (intValue < 91)
            {
                return intValue - 38;
            }
            if (intValue > 91)
            {
                return intValue - 96;
            }
            return -1;
        }
    }
}