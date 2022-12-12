using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day12
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var map = ParseMap(input);
            return PathFindFrom(map, 'S').First().Cost;
        }

        public object SolveTaskTwo(string[] input)
        {
            var map = ParseMap(input);
            var results = PathFindFrom(map, 'a');

            return results.OrderBy(x => x.Cost).First().Cost;
        }

        private static char[,] ParseMap(string[] input)
        {
            char[,] result = new char[input.Length, input[0].Length];
            for(int y = 0; y < input.Length; y++)
                for(int x = 0; x < input[y].Length; x++)
                    result[y, x] = input[y][x];

            return result;
        }

        private static List<Tile> PathFindFrom(char[,] map, char startLocation)
        {
            List<(int y, int x)> startLocations = new();
            for (int y = 0; y < map.GetLength(0); y++)
                for (int x = 0; x < map.GetLength(1); x++)
                    if (map[y, x].Equals(startLocation))
                        startLocations.Add((y, x));

            List<Tile> results = new();
            foreach (var start in startLocations)
            {
                try
                {
                    results.Add(PathFind(map, start));
                }
                catch { }
            }

            return results;
        }

        private static Tile PathFind(char[,] map, (int y, int x) startPosition)
        {
            var start = new Tile { Position = startPosition };
            var end = new Tile { Position = GetPositionOf(map, 'E') };

            start.SetDistance(end.Position);

            List<Tile> active = new()
            {
                start
            };

            HashSet<(int y, int x)> visited = new();

            while (active.Any())
            {
                var checkTile = active.OrderBy(x => x.CostDistance).First();

                // check if destination
                if (checkTile.Position == end.Position)
                    return checkTile;

                visited.Add(checkTile.Position);
                active.Remove(checkTile);

                var nexts = GetValidAdjecents(map, checkTile, end.Position);
                foreach(var next in nexts)
                {
                    if (visited.Contains(next.Position))
                        continue;

                    if (active.Any(x => x.Position == next.Position))
                    {
                        var existing = active.First(x => x.Position == next.Position);
                        if (existing.CostDistance > checkTile.CostDistance)
                        {
                            active.Remove(existing);
                            active.Add(next);
                        }
                    }
                    else
                    {
                        active.Add(next);
                    }
                }
            }

            throw new InvalidOperationException("no path found");
        }

        private static (int y, int x) GetPositionOf(char[,] map, char value)
        {
            for (int y = 0; y < map.GetLength(0); y++)
                for (int x = 0; x < map.GetLength(1); x++)
                    if (map[y, x].Equals(value))
                        return (y, x);

            return (-1, -1);
        }

        private static List<Tile> GetValidAdjecents(char[,] map, Tile current, (int y, int x) end)
        {
            var adjecents = new List<Tile>()
            {
                new Tile { Position = (current.Position.y - 1, current.Position.x), Cost = current.Cost + 1 },
                new Tile { Position = (current.Position.y + 1, current.Position.x), Cost = current.Cost + 1 },
                new Tile { Position = (current.Position.y, current.Position.x - 1), Cost = current.Cost + 1 },
                new Tile { Position = (current.Position.y, current.Position.x + 1), Cost = current.Cost + 1 },
            };

            adjecents.ForEach(x => x.SetDistance(end));

            List<char> validNextValues = new() { 'a' };
            if (map[current.Position.y, current.Position.x] != 'S')
                validNextValues = Enumerable.Range((int)'a', (map[current.Position.y, current.Position.x] + 1) - (int)'a' + 1).Select(x => (char)x).ToList();

            if (validNextValues.Contains('z'))
                validNextValues.Add('E');

            return adjecents
                    .Where(tile => tile.Position.y >= 0 && tile.Position.y <= map.GetLength(0) - 1)
                    .Where(tile => tile.Position.x >= 0 && tile.Position.x <= map.GetLength(1) - 1)
                    .Where(tile => validNextValues.Contains(map[tile.Position.y, tile.Position.x]))
                    .ToList();
        }

        private class Tile
        {
            public (int y, int x) Position { get; set; }
            public int Cost { get; set; }
            public int Distance { get; set; }

            public int CostDistance => Cost + Distance;

            public void SetDistance((int y, int x) target)
            {
                Distance = Math.Abs(target.x - Position.x) + Math.Abs(target.y - Position.y);
            }
        }
    }
}
