namespace AdventOfCodeCSharp.Day10
{
    public class Program
    {
        public static async Task Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var file = await File.ReadAllLinesAsync("input.txt");
            var variable = new Variable();
            // foreach (var line in file)
            // {
            //     var instructions = line.Split(" ");
            //     IncreaseCycle(instructions[0], variable);
            //     ProcessInstruction(instructions[0], instructions.Length > 1 ? int.Parse(instructions[1]) : 0, variable);
            // }
            // Console.WriteLine(variable.Result);

            variable = new Variable();
            variable.Cycle = 0;
            foreach (var line in file)
            {
                var instructions = line.Split(" ");
                IncreaseCycle(instructions[0], variable);
                ProcessInstruction(instructions[0], instructions.Length > 1 ? int.Parse(instructions[1]) : 0, variable);
            }
            for(int i = 0; i < 40; i ++)
            {
                Console.Write(variable.Pixels[i] ? "#" : ".");
            }
            Console.WriteLine();
            for(int i = 40; i < 80; i ++)
            {
                Console.Write(variable.Pixels[i] ? "#" : ".");
            }
            Console.WriteLine();
            for(int i = 80; i < 120; i ++)
            {
                Console.Write(variable.Pixels[i] ? "#" : ".");
            }
            Console.WriteLine();
            for(int i = 120; i < 160; i ++)
            {
                Console.Write(variable.Pixels[i] ? "#" : ".");
            }
            Console.WriteLine();
            for(int i = 160; i < 200; i ++)
            {
                Console.Write(variable.Pixels[i] ? "#" : ".");
            }
            Console.WriteLine();
            for(int i = 200; i < 240; i ++)
            {
                Console.Write(variable.Pixels[i] ? "#" : ".");
            }
            Console.WriteLine();
        }

        public static void IncreaseCycle(string instruction, Variable variable)
        {
            switch (instruction)
            {
                case "noop":
                    CheckForInterestingSignalStrength(variable);
                    PixelDrawing(variable);
                    variable.Cycle++;
                    break;
                case "addx":
                    CheckForInterestingSignalStrength(variable);
                    PixelDrawing(variable);
                    variable.Cycle++;
                    CheckForInterestingSignalStrength(variable);
                    PixelDrawing(variable);
                    variable.Cycle++;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void ProcessInstruction(string instruction, int parameter, Variable variable)
        {
            switch(instruction)
            {
                case "addx":
                    variable.Register += parameter;
                    break;
                default:
                    break;
            }
        }

        public static void CheckForInterestingSignalStrength(Variable variable)
        {
            switch (variable.Cycle)
            {
                case 20:
                case 60:
                case 100:
                case 140:
                case 180:
                case 220:
                    variable.Result += variable.Cycle * variable.Register;
                    break;
                default: 
                    break;
            }
        }

        public static void PixelDrawing(Variable variable)
        {
            if (variable.Cycle % 40 == variable.Register
                || variable.Cycle % 40 == variable.Register - 1
                || variable.Cycle % 40 == variable.Register + 1)
            {
                variable.Pixels[variable.Cycle] = true;
            }
        }
    }

    public class Variable
    {
        public int Cycle {get; set;} = 1;
        public int Register {get; set;} = 1;
        public int Result {get; set;} = 0;
        public bool[] Pixels {get; set;} = new bool[240];

        public Variable()
        {
            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i] = false;
            }
        }
    }
}