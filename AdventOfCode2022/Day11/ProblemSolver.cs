namespace AdventOfCode2022.Day11
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var monkeys = ParseMonkeys(input);
            PassAroundItems(monkeys);

            return monkeys.Select(x => x.InspectCount).OrderByDescending(x => x).Take(2).Aggregate((x1, x2) => x1 * x2);
        }

        public object SolveTaskTwo(string[] input)
        {
            var monkeys = ParseMonkeys(input);
            PassAroundItems(monkeys, 10000, false);

            return monkeys.Select(x => x.InspectCount).OrderByDescending(x => x).Take(2).Aggregate((x1, x2) => x1 * x2);
        }

        private static List<Monkey> ParseMonkeys(string[] input)
        {
            List<Monkey> monkeys = new();

            for (int i = 0; i < input.Length; i += 2)
            {
                List<long> items = input[++i][17..].Split(',').Select(long.Parse).ToList();
                string operation = input[++i][(input[i].IndexOf("=") + 2)..];
                int divisibleBy = int.Parse(input[++i][21..]);
                int trueTarget = int.Parse(input[++i][29..]);
                int falseTarget = int.Parse(input[++i][30..]);

                monkeys.Add(new Monkey(items, divisibleBy, operation, trueTarget, falseTarget));
            }

            return monkeys;
        }

        private static void PassAroundItems(List<Monkey> monkeys, int rounds = 20, bool step1 = true)
        {
            var relief = monkeys.Select(x => x.DivisibleBy).Aggregate((m1, m2) => m1 * m2);

            for (int x = 0; x < rounds; x++)
            {
                long[] inspectCounts = monkeys.Select(x => x.InspectCount).ToArray();
                for (int i = 0; i < monkeys.Count; i++)
                {
                    while (monkeys[i].Items.TryDequeue(out long item))
                    {
                        long inspectedItem = monkeys[i].InspectItem(item);
                        if (step1)
                            inspectedItem /= 3;
                        else
                            inspectedItem %= relief;

                        monkeys[i].InspectCount++;

                        int throwTo = monkeys[i].ThrowTo(inspectedItem);
                        monkeys[throwTo].Items.Enqueue(inspectedItem);
                    }
                }
            }
        }

        private class Monkey
        {
            public Queue<long> Items { get; } = new();

            public int DivisibleBy { get; }

            public string Operation { get; }

            public int TrueTarget { get; }

            public int FalseTarget { get; }

            public long InspectCount { get; set; } = 0;

            public Monkey(IEnumerable<long> items, int divisibleBy, string operation, int trueTarget, int falseTarget)
            {
                items.ToList().ForEach(Items.Enqueue);
                DivisibleBy = divisibleBy;
                Operation = operation;
                TrueTarget = trueTarget;
                FalseTarget = falseTarget;
            }

            public int ThrowTo(long item)
            {
                if (item % DivisibleBy == 0)
                    return TrueTarget;

                return FalseTarget;
            }

            public long InspectItem(long item)
            {
                string[] op = Operation.Split(' ');

                Func<long, long, long> math;
                if (op[1] == "*")
                    math = (x1, x2) => x1 * x2;
                else
                    math = (x1, x2) => x1 + x2;

                if (op[2] == "old")
                    return math(item, item);
                else
                    return math(item, long.Parse(op[2]));
            }
        }
    }
}
