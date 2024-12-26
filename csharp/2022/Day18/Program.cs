namespace AdventOfCodeCSharp.Day18
{
    public class Program
    {
        public async static Task Main()
        {
            var file = await File.ReadAllLinesAsync("input.txt");
            var cubes = new List<Cube>();
            foreach (var line in file)
            {
                var asdf = line.Split(",");
                cubes.Add(new Cube(int.Parse(asdf[0]), int.Parse(asdf[1]), int.Parse(asdf[2])));
            }

            var sides = 0;
            foreach (var cube in cubes)
            {
                var check = new Cube(cube.X + 1, cube.Y, cube.Z);
                if (!check.Exists(cubes)) sides++;
                check = new Cube(cube.X - 1, cube.Y, cube.Z);
                if (!check.Exists(cubes)) sides++;
                check = new Cube(cube.X, cube.Y + 1, cube.Z);
                if (!check.Exists(cubes)) sides++;
                check = new Cube(cube.X, cube.Y - 1, cube.Z);
                if (!check.Exists(cubes)) sides++;
                check = new Cube(cube.X, cube.Y, cube.Z + 1);
                if (!check.Exists(cubes)) sides++;
                check = new Cube(cube.X, cube.Y, cube.Z - 1);
                if (!check.Exists(cubes)) sides++;
            }

            // See https://aka.ms/new-console-template for more information
            Console.WriteLine(sides);
        }
    }

    public class Cube
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Cube(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public bool Exists(List<Cube> cubes)
        {
            return cubes.Any(_ => X == _.X && Y == _.Y && Z == _.Z);
        }
    }
}

