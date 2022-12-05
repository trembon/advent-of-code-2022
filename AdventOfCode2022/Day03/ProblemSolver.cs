namespace AdventOfCode2022.Day03
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var sharedItems = FindSharedItems(input);
            double priority = CalculatePriority(sharedItems);
            return priority;
        }

        public object SolveTaskTwo(string[] input)
        {
            var sharedItems = FindSharedItemsInThreeGroups(input);
            double priority = CalculatePriority(sharedItems);
            return priority;
        }

        private List<char> FindSharedItems(string[] input)
        {
            List<char> sharedItems = new(input.Length);
            foreach(string comp in input)
            {
                string first = comp.Substring(0, comp.Length / 2);
                string second = comp.Substring(comp.Length / 2);

                IEnumerable<char> shared = first.Where(x => second.Contains(x));
                sharedItems.Add(shared.First());
            }
            return sharedItems;
        }

        private double CalculatePriority(List<char> input)
        {
            double priority = 0;
            foreach(char c in input)
            {
                if (Char.IsUpper(c))
                    priority += (int)c - 64 + 26;
                else if(Char.IsLower(c))
                    priority += (int)c - 96;
            }
            return priority;
        }

        private List<char> FindSharedItemsInThreeGroups(string[] input)
        {
            List<char> sharedItems = new(input.Length / 3);
            for (int i = 0; i < input.Length; i += 3)
            {
                string first = input[i];
                string second = input[i + 1];
                string third = input[i + 2];

                IEnumerable<char> shared = first.Where(x => second.Contains(x) && third.Contains(x));
                sharedItems.Add(shared.First());
            }
            return sharedItems;
        }
    }
}
