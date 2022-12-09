namespace AdventOfCodeCSharp.Day09
{
    public class Program
    {
        public static async Task Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            Coordinate headPosition = new Coordinate(0, 0);
            Coordinate tailPosition = new Coordinate(0, 0);
            var tailCoordinates = new List<Coordinate>();
            tailCoordinates.Add(tailPosition.Clone());
            var file = await File.ReadAllLinesAsync("input.txt");
            foreach (var line in file)
            {
                var velocity = line.Split(' ');
                for (int i = 0; i < int.Parse(velocity[1]); i++)
                {
                    headPosition.Move(char.Parse(velocity[0]));
                    headPosition.Catchup(tailPosition);
                    if (!tailCoordinates.Any(_ => _.Equals(tailPosition)))
                    {
                        tailCoordinates.Add(tailPosition.Clone());
                    }
                }
            }
            Console.WriteLine(tailCoordinates.Count());

            tailCoordinates = new List<Coordinate>();
            // Part 2
            var coordinateList = new List<Coordinate>();
            for (int i = 0; i < 10; i++)
            {
                coordinateList.Add(new Coordinate(0,0));
            }
            foreach (var line in file)
            {
                var velocity = line.Split(' ');
                for (int i = 0; i < int.Parse(velocity[1]); i++)
                {
                    coordinateList[0].Move(char.Parse(velocity[0]));
                    for(int j = 1; j < coordinateList.Count(); j++)
                    {
                        coordinateList[j-1].Catchup(coordinateList[j]);
                    }
                    var tail = coordinateList.Last();
                    if (!tailCoordinates.Any(_ => _.Equals(tail)))
                    {
                        tailCoordinates.Add(tail.Clone());
                    }
                }
            }

            Console.WriteLine(tailCoordinates.Count());
        }
    }

    public class Coordinate 
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object o)
        {
            if (o is Coordinate)
            {
                return this.Equals((Coordinate)o);
            }
            return base.Equals(o);
        }

        public bool Equals(Coordinate c)
        {
            if (this.X == c.X
                && this.Y == c.Y)
                {
                    return true;
                }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Move(char direction)
        {
            switch (direction)
            {
                case 'R':
                    X += 1;
                    break;
                case 'L':
                    X += -1;
                    break;
                case 'U':
                    Y += 1;
                    break;
                case 'D':
                    Y += -1;
                    break;
            }
        }

        public int xDistance(Coordinate tail)
        {
            return X - tail.X;
        }

        public int yDistance(Coordinate tail)
        {
            return Y - tail.Y;
        }

        public void Catchup(Coordinate tail)
        {
            if (Math.Abs(xDistance(tail)) > 1 && Math.Abs(yDistance(tail)) > 1)
            {
                tail.X += xDistance(tail) / 2;
                tail.Y += yDistance(tail) / 2;
            }
            else if (Math.Abs(xDistance(tail)) > 1)
            {
                tail.X += xDistance(tail) / 2;
                if (Math.Abs(yDistance(tail)) == 1)
                {
                    tail.Y += yDistance(tail);
                }
            }
            else if (Math.Abs(yDistance(tail)) > 1)
            {
                tail.Y += yDistance(tail) / 2;
                if (Math.Abs(xDistance(tail)) == 1)
                {
                    tail.X += xDistance(tail);
                }
            }
        }

        public Coordinate Clone()
        {
            return new Coordinate(X, Y);
        }
    }
}