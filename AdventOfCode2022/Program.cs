using AdventOfCode2022;
using System.Reflection;

string day = DateTime.Today.Day.ToString("D2");
//string day = "01";

Console.WriteLine($"### Solving AdventOfCode 2022 - Day {day} ###");
Console.WriteLine();

var assembly = Assembly.GetExecutingAssembly();

// read file
List<string> input = new();
using (var stream = assembly.GetManifestResourceStream($"AdventOfCode2022.Day{day}.input.txt"))
{
    if(stream == null)
    {
        Console.WriteLine("Unable to find the input for today");
        return;
    }

    using var reader = new StreamReader(stream);
    string? line;
    while ((line = reader.ReadLine()) != null)
        input.Add(line);
}


// find todays solver
var problemSolver =  assembly.CreateInstance($"AdventOfCode2022.Day{day}.ProblemSolver") as IProblemSolver;
if(problemSolver == null)
{
    Console.WriteLine("Unable to find the solver for today");
    return;
}

// execute the code
var inputArray = input.ToArray();
var watch = new System.Diagnostics.Stopwatch();

try
{
    watch.Start();
    double taskOneSolution = await problemSolver.SolveTaskOne(inputArray);
    watch.Stop();
    Console.WriteLine($"Solution for task 1 ({watch.ElapsedMilliseconds}ms): {taskOneSolution}");
}
catch(Exception ex)
{
    Console.WriteLine($"Task 1 error: {ex.Message}");
}

Console.WriteLine();

try
{
    watch.Restart();
    double taskTwoSolution = await problemSolver.SolveTaskTwo(inputArray);
    watch.Stop();
    Console.WriteLine($"Solution for task 2 ({watch.ElapsedMilliseconds}ms): {taskTwoSolution}");
}
catch (Exception ex)
{
    Console.WriteLine($"Task 2 error: {ex.Message}");
}
