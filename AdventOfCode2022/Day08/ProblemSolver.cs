namespace AdventOfCode2022.Day08
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var trees = ParseTreeMapping(input);
            var visibleTrees = CalculateVisibleTrees(trees);
            return visibleTrees.Count;
        }

        public object SolveTaskTwo(string[] input)
        {
            var trees = ParseTreeMapping(input);
            var scores = CalculateScenicScore(trees);
            return scores.Values.Max();
        }

        private static int[,] ParseTreeMapping(string[] input)
        {
            int[,] result = new int[input.Length,input[0].Length];

            for(int y = 0; y < input.Length; y++)
                for(int x = 0; x < input[y].Length; x++)
                    result[y, x] = int.Parse(input[y][x].ToString());

            return result;
        }

        private static HashSet<string> CalculateVisibleTrees(int[,] trees)
        {
            HashSet<string> result = new();

            // calculate visible from top view
            for(int x = 0; x < trees.GetLength(1); x++)
            {
                result.Add($"0,{x}");
                int max = trees[0, x];
                for(int y = 1; y < trees.GetLength(0); y++)
                {
                    if (trees[y,x] > max)
                    {
                        result.Add($"{y},{x}");
                        max = trees[y, x];
                    }
                }
            }

            // calculate visible from bottom view
            for (int x = 0; x < trees.GetLength(1); x++)
            {
                result.Add($"{trees.GetLength(0) - 1},{x}");
                int max = trees[trees.GetLength(0) - 1, x];
                for (int y = trees.GetLength(0) - 2; y >= 0; y--)
                {
                    if (trees[y, x] > max)
                    {
                        result.Add($"{y},{x}");
                        max = trees[y, x];
                    }
                }
            }

            // calculate visible from left view
            for (int y = 0; y < trees.GetLength(0); y++)
            {
                result.Add($"{y},0");
                int max = trees[y, 0];
                for (int x = 1; x < trees.GetLength(1); x++)
                {
                    if (trees[y, x] > max)
                    {
                        result.Add($"{y},{x}");
                        max = trees[y, x];
                    }
                }
            }

            // calculate visible from right view
            for (int y = 0; y < trees.GetLength(0); y++)
            {
                result.Add($"{y},{trees.GetLength(1) - 1}");
                int max = trees[y, trees.GetLength(1) - 1];
                for (int x = trees.GetLength(1) - 2; x >= 0; x--)
                {
                    if (trees[y, x] > max)
                    {
                        result.Add($"{y},{x}");
                        max = trees[y, x];
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, int> CalculateScenicScore(int[,] trees)
        {
            Dictionary<string, int> result = new();
            for(int y = 1; y < trees.GetLength(0) - 1; y++)
                for(int x = 1; x < trees.GetLength(1) - 1; x++)
                    result.Add($"{y},{x}", CalculateScenicScore(trees, y, x));

            return result;
        }

        private static int CalculateScenicScore(int[,] trees, int row, int col)
        {
            int score = 0;

            // calculate down
            int down = 0;
            for (int y = row + 1; y < trees.GetLength(0); y++)
            {
                down++;

                if (trees[row, col] <= trees[y, col])
                    break;
            }
            score += down;

            // calculate up
            int up = 0;
            for (int y = row - 1; y >= 0; y--)
            {
                up++;

                if (trees[row, col] <= trees[y, col])
                    break;
            }
            score *= up;

            // calculate right
            int right = 0;
            for (int x = col + 1; x < trees.GetLength(1); x++)
            {
                right++;

                if (trees[row, col] <= trees[row, x])
                    break;
            }
            score *= right;

            // calculate left
            int left = 0;
            for (int x = col - 1; x >= 0; x--)
            {
                left++;

                if (trees[row, col] <= trees[row, x])
                    break;
            }
            score *= left;

            return score;
        }
    }
}
