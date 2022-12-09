using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day09
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var positions = MoveHeadAndTail(input);
            return positions.Count;
        }

        public object SolveTaskTwo(string[] input)
        {
            var positions = MoveHeadAndNineTail(input);
            return positions.Count;
        }

        private static HashSet<string> MoveHeadAndTail(string[] input)
        {
            HashSet<string> result = new();

            (int y, int x) head = new(0, 0);
            (int y, int x) tail = new(0, 0);
            result.Add($"0,0");

            foreach (string line in input)
            {
                string[] move = line.Split(' ');
                int count = int.Parse(move[1]);

                for (int i = 0; i < count; i++)
                {
                    switch (move[0])
                    {
                        case "U":
                            head.y--;
                            break;

                        case "D":
                            head.y++;
                            break;

                        case "R":
                            head.x++;
                            break;

                        case "L":
                            head.x--;
                            break;
                    }

                    if (!IsTailAdjecent(head, tail))
                    {
                        tail = MoveTail(head, tail);
                        result.Add($"{tail.y},{tail.x}");
                    }
                }
            }

            return result;
        }

        private static HashSet<string> MoveHeadAndNineTail(string[] input)
        {
            HashSet<string> result = new();

            (int y, int x) head = new(0, 0);
            result.Add($"0,0");

            List<(int y, int x)> tails = new(9);
            for (int i = 0; i < 9; i++)
                tails.Add(new(0, 0));

            foreach (string line in input)
            {
                string[] move = line.Split(' ');
                int count = int.Parse(move[1]);

                for (int i = 0; i < count; i++)
                {
                    switch (move[0])
                    {
                        case "U":
                            head.y--;
                            break;

                        case "D":
                            head.y++;
                            break;

                        case "R":
                            head.x++;
                            break;

                        case "L":
                            head.x--;
                            break;
                    }

                    for (int t = 0; t < tails.Count; t++)
                        if (!IsTailAdjecent(t == 0 ? head : tails[t - 1], tails[t]))
                            tails[t] = MoveTailV2(t == 0 ? head : tails[t - 1], tails[t]);

                    result.Add($"{tails[8].y},{tails[8].x}");
                }
            }

            return result;
        }

        private static bool IsTailAdjecent((int y, int x) head, (int y, int x) tail)
        {
            int yDiff = head.y - tail.y;
            int xDiff = head.x - tail.x;

            yDiff = Math.Abs(yDiff);
            xDiff = Math.Abs(xDiff);

            return yDiff <= 1 && xDiff <= 1;
        }

        private static (int y, int x) MoveTail((int y, int x) head, (int y, int x) tail)
        {
            int yDiff = head.y - tail.y;
            int xDiff = head.x - tail.x;

            int yAbs = Math.Abs(yDiff);
            int xAbs = Math.Abs(xDiff);

            if (yAbs > 1 && yAbs > xAbs)
            {
                tail.y += Math.Sign(yDiff);
                tail.x = head.x;
            }
            if (xAbs > 1 && xAbs > yAbs)
            {
                tail.x += Math.Sign(xDiff);
                tail.y = head.y;
            }

            return tail;
        }

        private static (int y, int x) MoveTailV2((int y, int x) head, (int y, int x) tail)
        {
            if (tail.y < head.y - 1)
            {
                tail.y += 1;

                if (tail.x != head.x)
                {
                    if (tail.x < head.x)
                        tail.x += 1;
                    else
                        tail.x -= 1;
                }
            }
            else if (tail.y > head.y + 1)
            {
                tail.y -= 1;

                if (tail.x != head.x)
                {
                    if (tail.x < head.x)
                        tail.x += 1;
                    else
                        tail.x -= 1;
                }
            }
            else if (tail.x < head.x - 1)
            {
                tail.x += 1;

                if (tail.y != head.y)
                {
                    if (tail.y < head.y)
                        tail.y += 1;
                    else
                        tail.y -= 1;
                }
            }
            else if (tail.x > head.x + 1)
            {
                tail.x -= 1;

                if (tail.y != head.y)
                {
                    if (tail.y < head.y)
                        tail.y += 1;
                    else
                        tail.y -= 1;
                }
            }

            return tail;
        }
    }
}
