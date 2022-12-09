using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day05
{
    internal class ProblemSolver : IProblemSolver
    {
        private static Regex moveRegex = new("(\\d+)", RegexOptions.Compiled | RegexOptions.Singleline);

        public object SolveTaskOne(string[] input)
        {
            var boxes = ParseStartingPoint(input);
            var moves = ParseMoves(input);

            MoveBoxes(boxes, moves);

            return GetBoxesTopString(boxes);
        }

        public object SolveTaskTwo(string[] input)
        {
            var boxes = ParseStartingPoint(input);
            var moves = ParseMoves(input);

            MoveBoxes9001(boxes, moves);

            return GetBoxesTopString(boxes);
        }

        private Dictionary<char, Stack<char>> ParseStartingPoint(string[] input)
        {
            var boxes = new Dictionary<char, Stack<char>>();

            for(int i = 0; i < input.Length; i++)
            {
                if (int.TryParse(input[i].Replace(" ", ""), out _))
                {
                    for(int x = 0; x < input[i].Length; x++)
                    {
                        if (char.IsDigit(input[i][x]))
                        {
                            if (!boxes.ContainsKey(input[i][x]))
                                boxes[input[i][x]] = new Stack<char>();

                            for(int y =  i - 1; y >= 0; y--)
                            {
                                if (input[y].Length >= x && char.IsUpper(input[y][x]))
                                    boxes[input[i][x]].Push(input[y][x]);
                            }
                        }
                    }

                    break;
                }
            }

            return boxes;
        }

        private List<(char from, char to, int count)> ParseMoves(string[] input)
        {
            List<(char from, char to, int count)> result = new();

            foreach (var line in input)
            {
                if (!line.StartsWith("move"))
                    continue;

                var matches = moveRegex.Matches(line);
                result.Add(new(matches[1].Value[0], matches[2].Value[0], int.Parse(matches[0].Value)));
            }

            return result;
        }

        private void MoveBoxes(Dictionary<char, Stack<char>> boxes, List<(char from, char to, int count)> moves)
        {
            foreach(var move in moves)
            {
                for(int i = 0; i < move.count; i++)
                {
                    char item = boxes[move.from].Pop();
                    boxes[move.to].Push(item);
                }
            }
        }

        private void MoveBoxes9001(Dictionary<char, Stack<char>> boxes, List<(char from, char to, int count)> moves)
        {
            foreach (var move in moves)
            {
                List<char> stack = new List<char>();
                for (int i = 0; i < move.count; i++)
                    stack.Add(boxes[move.from].Pop());

                stack.Reverse();
                foreach (char item in stack)
                    boxes[move.to].Push(item);
            }
        }

        private string GetBoxesTopString(Dictionary<char, Stack<char>> boxes)
        {
            List<char> chars = new(boxes.Count);
            foreach (var box in boxes)
                chars.Add(box.Value.Pop());

            return new string(chars.ToArray());
        }
    }
}
