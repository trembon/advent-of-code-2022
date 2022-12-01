namespace AdventOfCode2022
{
    internal interface IProblemSolver
    {
        Task<double> SolveTaskOne(string[] input);
        Task<double> SolveTaskTwo(string[] input);
    }
}
