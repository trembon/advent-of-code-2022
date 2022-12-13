using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day13
{
    internal class ProblemSolver : IProblemSolver
    {
        public object SolveTaskOne(string[] input)
        {
            var packets = ParsePackets(input);
            var sortedIndexes = FindSortedIndexes(packets);

            return sortedIndexes.Sum();
        }

        public object SolveTaskTwo(string[] input)
        {
            var inputWithExtraPackets = input.ToList();
            inputWithExtraPackets.AddRange(new string[] { "", "[[2]]", "[[6]]" });

            var packets = ParsePackets(inputWithExtraPackets.ToArray());
            var packetFlatList = packets.Select(x => new Packet[] { x.p1, x.p2 }).SelectMany(x => x).ToList();

            var sortedPackets = packetFlatList.OrderBy(x => x, new PacketComparer()).Select(x => x.ToString()).ToList();

            int index1 = sortedPackets.IndexOf("[[2]]") + 1;
            int index2 = sortedPackets.IndexOf("[[6]]") + 1;

            return index1 * index2;
        }

        private static List<(Packet p1, Packet p2)> ParsePackets(string[] input)
        {
            Packet ParsePacket(string line)
            {
                string buffer = "";
                PacketList? current = new PacketList(null);
                for(int c = 0; c < line.Length; c++)
                {
                    if (line[c] == '[')
                    {
                        current = new PacketList(current);
                        current?.Parent?.Values.Add(current);
                    }
                    else if (line[c] == ']' || line[c] == ',')
                    {
                        if (buffer != "")
                        {
                            current?.Values.Add(new PacketItem(int.Parse(buffer), current));
                            buffer = "";
                        }

                        if(line[c] == ']')
                            current = current?.Parent;
                    }
                    else
                    {
                        buffer += line[c];
                    }
                }

                if (current == null)
                    throw new InvalidOperationException($"Unable to parse '{line}'");

                return current?.Values.FirstOrDefault();
            }

            List<(Packet p1, Packet p2)> result = new();
            for (int i = 0; i < input.Length; i++)
            {
                var p1 = ParsePacket(input[i]);
                var p2 = ParsePacket(input[++i]);
                i++;

                result.Add((p1, p2));
            }
            return result;
        }

        private static List<int> FindSortedIndexes(List<(Packet p1, Packet p2)> packetPairs)
        {
            List<int> sortedIndexes = new();

            for (int i = 0; i < packetPairs.Count; i++)
                if (packetPairs[i].p1.Compare(packetPairs[i].p2) == -1)
                    sortedIndexes.Add(i + 1);

            return sortedIndexes;
        }

        private class PacketComparer : IComparer<Packet>
        {
            public int Compare(Packet? x, Packet? y)
            {
                return x.Compare(y);
            }
        }

        private abstract class Packet
        {
            public PacketList? Parent { get; }

            public Packet(PacketList? parent)
            {
                Parent = parent;
            }

            public abstract int Compare(Packet p);
        }

        private class PacketList : Packet
        {
            public List<Packet> Values { get; } = new();

            public PacketList(PacketList? parent) : base(parent)
            {
            }
            public PacketList(PacketList? parent, Packet singleValue) : base(parent)
            {
                Values.Add(singleValue);
            }

            public override string ToString()
            {
                return $"[{string.Join(",", Values)}]";
            }

            public override int Compare(Packet compare)
            {
                PacketList r = compare as PacketList;
                if (compare is PacketItem)
                    r = new PacketList(compare.Parent, compare);

                int length = int.Max(Values.Count, r.Values.Count);
                int? lastResult = null;
                for(int i = 0; i < length; i++)
                {
                    if (i >= Values.Count)
                        return -1;

                    if (i >= r.Values.Count)
                        return !lastResult.HasValue || lastResult == 0 ? 1 : -1;

                    lastResult = Values[i].Compare(r.Values[i]);
                    if (lastResult != 0)
                        return lastResult.Value;
                }

                if (Values.Count() == r.Values.Count())
                    return 0;

                return -1;
            }
        }

        private class PacketItem : Packet
        {
            public int Value { get; }

            public PacketItem(int value, PacketList? parent) :  base(parent)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value.ToString();
            }

            public override int Compare(Packet r)
            {
                if (r is PacketList)
                    return new PacketList(this.Parent, this).Compare(r);

                var item = r as PacketItem;
                return Value.CompareTo(item.Value);
            }
        }
    }
}
