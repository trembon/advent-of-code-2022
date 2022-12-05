namespace AdventOfCode2022.Day01
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var elfs = GetElfCalories(input);
            return elfs.Max();
        }

        public object SolveTaskTwo(string[] input)
        {
            var elfs = GetElfCalories(input);
            var top3 = elfs.OrderByDescending(x => x).Take(3).ToArray();
            return top3.Sum();
        }

        private static List<long> GetElfCalories(string[] input)
        {
            List<long> result = new();

            long calories = 0;
            foreach (var item in input)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    result.Add(calories);
                    calories = 0;
                }
                else
                {
                    long data = long.Parse(item);
                    calories += data;
                }
            }

            if(calories > 0)
                result.Add(calories);

            return result;
        }
    }
}
