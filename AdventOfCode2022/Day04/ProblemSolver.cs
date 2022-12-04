namespace AdventOfCode2022.Day04
{
    internal class ProblemSolver : IProblemSolver
    {
        public Task<double> SolveTaskOne(string[] input)
        {
            var segments = CalculateSegments(input);
            int result = CheckDuplicateSegments(segments);
            return Task.FromResult((double)result);
        }

        public Task<double> SolveTaskTwo(string[] input)
        {
            var segments = CalculateSegments(input);
            int result = CheckAnyDuplicateSegments(segments);
            return Task.FromResult((double)result);
        }

        private List<(IEnumerable<int> first, IEnumerable<int> second)> CalculateSegments(string[] input)
        {
            List<(IEnumerable<int> first, IEnumerable<int> second)> result = new(input.Length);
            foreach(var data in input)
            {
                string[] elfs = data.Split(',');

                int first = int.Parse(elfs[0].Split('-')[0]);
                int second = int.Parse(elfs[0].Split('-')[1]);
                var firstSegment = Enumerable.Range(first, second - first + 1);

                first = int.Parse(elfs[1].Split('-')[0]);
                second = int.Parse(elfs[1].Split('-')[1]);
                var secondSegment = Enumerable.Range(first, second - first + 1);

                result.Add((firstSegment, secondSegment));
            }
            return result;
        }

        private int CheckDuplicateSegments(List<(IEnumerable<int> first, IEnumerable<int> second)> input)
        {
            int duplicateSegments = 0;
            foreach(var segment in input)
            {
                if(segment.first.Count() > segment.second.Count())
                {
                    if(segment.second.All(x => segment.first.Contains(x)))
                        duplicateSegments++;
                }
                else
                {
                    if (segment.first.All(x => segment.second.Contains(x)))
                        duplicateSegments++;
                }
            }
            return duplicateSegments;
        }

        private int CheckAnyDuplicateSegments(List<(IEnumerable<int> first, IEnumerable<int> second)> input)
        {
            int duplicateSegments = 0;
            foreach (var segment in input)
            {
                if (segment.first.Any(x => segment.second.Contains(x)))
                        duplicateSegments++;
            }
            return duplicateSegments;
        }
    }
}
