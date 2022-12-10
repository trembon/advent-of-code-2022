using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var result = CalculateSignalStrength(input);
            return result.Values.Sum();
        }

        public object SolveTaskTwo(string[] input)
        {
            return RenderCRT(input);
        }

        private static Dictionary<int, int> CalculateSignalStrength(string[] input)
        {
            Dictionary<int, int> result = new();

            int x = 1;
            int cycle = 0;
            foreach(var cmd in input)
            {
                if (cmd == "noop")
                {
                    IncreaseCycle(ref cycle, x, result);
                }
                else if (cmd.StartsWith("addx"))
                {
                    for(int i = 0; i < 2; i++)
                        IncreaseCycle(ref cycle, x, result);
                    
                    x += int.Parse(cmd[5..]);
                }
            }

            return result;
        }

        private static void IncreaseCycle(ref int cycle, int x, Dictionary<int, int > result)
        {
            cycle++;

            if (cycle % 40 == 20)
                result.Add(cycle, x * cycle);
        }

        private static StringBuilder RenderCRT(string[] input)
        {
            StringBuilder result = new();
            result.AppendLine("");

            int x = 1;
            int cycle = 0;
            foreach (var cmd in input)
            {
                if (cmd == "noop")
                {
                    RenderPixel(ref cycle, x, result);
                }
                else if (cmd.StartsWith("addx"))
                {
                    for (int i = 0; i < 2; i++)
                        RenderPixel(ref cycle, x, result);

                    x += int.Parse(cmd[5..]);
                }
            }

            return result;
        }

        private static void RenderPixel(ref int cycle, int x, StringBuilder builder)
        {
            cycle++;

            int cycleCompare = cycle % 40;
            if (cycleCompare == 0)
                cycleCompare = 40;

            if (cycleCompare >= x && cycleCompare <= x + 2)
                builder.Append('#');
            else
                builder.Append('.');

            if (cycle % 40 == 0)
                builder.AppendLine("");
        }
    }
}
