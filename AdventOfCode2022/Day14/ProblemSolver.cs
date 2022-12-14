namespace AdventOfCode2022.Day14
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var map = ParseMap(input);
            LetSandFall(map);
            return map.Values.Count(x => x == '+');
        }

        public object SolveTaskTwo(string[] input)
        {
            var map = ParseMap(input);
            AddBottomFloor(map);
            LetSandFall(map);
            return map.Values.Count(x => x == '+');
        }

        private static Dictionary<(int x, int y), char> ParseMap(string[] input)
        {
            Dictionary<(int x, int y), char> result = new();

            foreach (var line in input)
            {
                string[] paths = line.Split(" -> ");

                int currentX = int.Parse(paths[0].Split(',')[0]);
                int currentY = int.Parse(paths[0].Split(',')[1]);
                result[(currentX, currentY)] = '#';

                for(int i = 1; i < paths.Length; i++)
                {
                    int toX = int.Parse(paths[i].Split(',')[0]);
                    int toY = int.Parse(paths[i].Split(',')[1]);

                    int diffX = toX - currentX;
                    int diffY = toY - currentY;

                    for(int x = 0; x < Math.Abs(diffX); x++)
                    {
                        currentX += Math.Sign(diffX);
                        result[(currentX, currentY)] = '#';
                    }

                    for (int y = 0; y < Math.Abs(diffY); y++)
                    {
                        currentY += Math.Sign(diffY);
                        result[(currentX, currentY)] = '#';
                    }
                }
            }

            return result;
        }

        private static void LetSandFall(Dictionary<(int x, int y), char> map)
        {
            int maxY = map.Keys.Select(x => x.y).Max();

            while (true)
            {
                int x = 500;
                int y = 0;

                while (Move(map, ref x, ref y) && maxY >= y) ;

                if (y > maxY)
                    break;

                map.Add((x, y), '+');

                if (x == 500 && y == 0)
                    break;
            }
        }

        private static bool Move(Dictionary<(int x, int y), char> map, ref int x, ref int y)
        {
            // move down
            if(!map.ContainsKey((x, y + 1)))
            {
                y++;
                return true;
            }

            // move down left
            if (!map.ContainsKey((x - 1, y + 1)))
            {
                x--;
                y++;
                return true;
            }

            // move down right
            if (!map.ContainsKey((x + 1, y + 1)))
            {
                x++;
                y++;
                return true;
            }

            return false;
        }

        private static void AddBottomFloor(Dictionary<(int x, int y), char> map)
        {
            int floor = map.Keys.Select(x => x.y).Max() + 2;

            for (int x = 500 - (floor * 2); x < 500 + (floor * 2); x++)
                map[(x, floor)] = '#';
        }
    }
}
