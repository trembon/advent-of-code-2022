namespace AdventOfCode2022.Day06
{
    internal class ProblemSolver : IProblemSolver
    {
        private const int START_OF_PACKET_LENGTH = 4;
        private const int START_OF_MESSAGE_LENGTH = 14;

        public object SolveTaskOne(string[] input)
        {
            return GetStartOfPacketPosition(input.First(), START_OF_PACKET_LENGTH);
        }

        public object SolveTaskTwo(string[] input)
        {
            return GetStartOfPacketPosition(input.First(), START_OF_MESSAGE_LENGTH);
        }

        private static int GetStartOfPacketPosition(string input, int start_length)
        {
            for(int i = 0; i < input.Length - start_length; i++)
                if (input.Substring(i, start_length).Distinct().Count() == start_length)
                    return i + start_length;

            return -1;
        }
    }
}
