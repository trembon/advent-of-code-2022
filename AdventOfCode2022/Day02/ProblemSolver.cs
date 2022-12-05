namespace AdventOfCode2022.Day02
{
    internal class ProblemSolver : IProblemSolver
    {
        private static Dictionary<char, double> scoreMapping = new Dictionary<char, double>
        {
            { 'A', 1 },
            { 'B', 2 },
            { 'C', 3 }
        };

        private static Dictionary<char, char> loseMapping = new Dictionary<char, char>
        {
            { 'A', 'B' },
            { 'B', 'C' },
            { 'C', 'A' }
        };

        private static Dictionary<char, char> winMapping = new Dictionary<char, char>
        {
            { 'A', 'C' },
            { 'B', 'A' },
            { 'C', 'B' }
        };

        public object SolveTaskOne(string[] input)
        {
            var parsed = ParseInput(input);
            double score = CalculateScore(parsed, new Dictionary<char, char>
            {
                { 'X', 'A' },
                { 'Y', 'B' },
                { 'Z', 'C' },
            });
            return score;
        }

        public object SolveTaskTwo(string[] input)
        {
            var parsed = ParseInput(input);
            double score = CalculateScoreStep2(parsed);
            return score;
        }

        private List<Tuple<char, char>> ParseInput(string[] input)
        {
            List<Tuple<char, char>> values = new();
            foreach (var line in input)
            {
                string[] split = line.Split(' ');
                values.Add(new Tuple<char, char>(split[0][0], split[1][0]));
            }
            return values;
        }

        private static double CalculateScore(List<Tuple<char, char>> values, Dictionary<char, char> mapping)
        {
            double score = 0;
            foreach (var value in values)
            {
                if (value.Item1 == 'A')
                {
                    if (mapping[value.Item2] == 'A')
                    {
                        score += 3 + 1;
                    }
                    else if(mapping[value.Item2] == 'B')
                    {
                        score += 6 + 2;
                    }
                    else if (mapping[value.Item2] == 'C')
                    {
                        score += 0 + 3;
                    }
                }
                else if (value.Item1 == 'B')
                {
                    if (mapping[value.Item2] == 'A')
                    {
                        score += 0 + 1;
                    }
                    else if (mapping[value.Item2] == 'B')
                    {
                        score += 3 + 2;
                    }
                    else if (mapping[value.Item2] == 'C')
                    {
                        score += 6 + 3;
                    }
                }
                else if (value.Item1 == 'C')
                {
                    if (mapping[value.Item2] == 'A')
                    {
                        score += 6 + 1;
                    }
                    else if (mapping[value.Item2] == 'B')
                    {
                        score += 0 + 2;
                    }
                    else if (mapping[value.Item2] == 'C')
                    {
                        score += 3 + 3;
                    }
                }
            }
            return score;
        }

        private static double CalculateScoreStep2(List<Tuple<char, char>> values)
        {
            double score = 0;
            foreach (var value in values)
            {
                if (value.Item2 == 'X')
                {
                    score += 0 + scoreMapping[winMapping[value.Item1]];
                }
                else if (value.Item2 == 'Y')
                {
                    score += 3 + scoreMapping[value.Item1];
                }
                else if (value.Item2 == 'Z')
                {
                    score += 6 + scoreMapping[loseMapping[value.Item1]];
                }
            }
            return score;
        }
    }
}
