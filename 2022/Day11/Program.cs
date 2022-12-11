using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Numerics;

namespace AdventOfCodeCSharp.Day11
{
    public class Program
    {
        public async static Task Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var file = await File.ReadAllLinesAsync("input.txt");
            var monkeys = new List<Monkey>();

            for (long i = 0; i < file.Length; i += 7)
            {
                var monkey = new Monkey()
                {
                    Items = file[i+1].Replace("  Starting items: ", "").Split(", ").Select(_ => BigInteger.Parse(_)).ToList(),
                    Operation = file[i+2].Replace("  Operation: new = ", ""),
                    TestDivisibleBy = long.Parse(file[i+3].Replace("  Test: divisible by ", "")),
                    TestTrueResultMonkeyNumber = long.Parse(file[i+4].Replace("    If true: throw to monkey ", "")),
                    TestFalseResultMonkeyNumber = long.Parse(file[i+5].Replace("    If false: throw to monkey ",""))
                };
                monkeys.Add(monkey);
            }

var divisibleByAll = monkeys.First().TestDivisibleBy;
            for (int i = 1; i < monkeys.Count(); i++)
            {
                divisibleByAll *= monkeys[i].TestDivisibleBy;
            }
            for (long i = 0; i < 10000; i++)
            {
                if ((i % 1000 == 0 || i == 20 || i == 1) && i != 0)
                {
                    int j = 0;
                }
                foreach (var monkey in monkeys)
                {
                    foreach (var item in monkey.Items)
                    {
                        var itemNew = monkey.ProcessOperation(item);
                        // part 1
                        //itemNew = itemNew / 3;
                        // part 2
                        itemNew = itemNew % divisibleByAll;
                        if (itemNew % monkey.TestDivisibleBy == 0)
                        {
                            monkeys[(int)monkey.TestTrueResultMonkeyNumber].Items.Add(itemNew);
                        }
                        else 
                        {
                            monkeys[(int)monkey.TestFalseResultMonkeyNumber].Items.Add(itemNew);
                        }
                        monkey.InspectionCount++;
                    }
                    monkey.Items = new List<BigInteger>();
                }
            }
            var monkeyBusiness = monkeys.OrderByDescending(_ => _.InspectionCount).Take(2).Select(_ => _.InspectionCount).ToList();
            Console.WriteLine((long)monkeyBusiness[0] * (long)monkeyBusiness[1]);
        }
    }

    [DebuggerDisplay("Monkey Business: {InspectionCount}")]
    public class Monkey 
    {
        public List<BigInteger> Items {get; set;}
        public string Operation {get; set;}
        public long TestDivisibleBy {get; set;}
        public long TestTrueResultMonkeyNumber {get; set;}
        public long TestFalseResultMonkeyNumber {get; set;}
        public long InspectionCount {get; set;}

        public BigInteger ProcessOperation(BigInteger item)
        {
            var partsOfOperation = Operation.Split(" ");
            var leftPart = ParsePartOfOperation(partsOfOperation[0], item);
            var rightPart = ParsePartOfOperation(partsOfOperation[2], item);
            return OperatedOn(leftPart, rightPart, partsOfOperation[1]);
        }

        public BigInteger ParsePartOfOperation(string part, BigInteger item)
        {
            if (part == "old")
            {
                return item;
            }
            return long.Parse(part);
        }

        public BigInteger OperatedOn(BigInteger leftPart, BigInteger rightPart, string operation)
        {
            if (operation == "*")
            {
                
                return BigInteger.Multiply(leftPart, rightPart);
            }
            else if (operation == "+")
            {
                return BigInteger.Add(leftPart, rightPart);
            }
            throw new NotImplementedException();
        }
    }
}

