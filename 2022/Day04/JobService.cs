namespace AdventOfCodeCSharp.Day04
{
    public static class JobService
    {
        public static bool DetermineSectionEncompassAll(string elf1, string elf2)
        {
            var section1 = elf1.Split("-");
            var section1Start = int.Parse(section1[0]);
            var section1End = int.Parse(section1[1]);
            var section2 = elf2.Split("-");
            var section2Start = int.Parse(section2[0]);
            var section2End = int.Parse(section2[1]);
            if ((section1Start <= section2Start
                && section1End >= section2End)
                || (section2Start <= section1Start
                && section2End >= section1End))
                {
                    return true;
                }
            return false;
        }

        public static bool DetermineSectionEncompassAny(string elf1, string elf2)
        {
            var section1 = elf1.Split("-");
            var section1Start = int.Parse(section1[0]);
            var section1End = int.Parse(section1[1]);
            var section2 = elf2.Split("-");
            var section2Start = int.Parse(section2[0]);
            var section2End = int.Parse(section2[1]);
            if ((section1Start <= section2Start && section1End >= section2Start)
                || (section1Start <= section2End && section1End >= section2End)
                || (section2Start <= section1Start && section2End >= section1Start)
                || (section2Start <= section1End && section2End >= section1End))
                {
                    return true;
                }
            return false;
        }
    }
}