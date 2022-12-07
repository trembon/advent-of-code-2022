namespace AdventOfCode2022.Day07
{
    internal class ProblemSolver : IProblemSolver
    {
        private const int TOTAL_DISK_SIZE = 70000000;
        private const int UPDATE_SIZE = 30000000;

        public object SolveTaskOne(string[] input)
        {
            var folders = ExecuteCommands(input);
            var folderWithFilteredSize = folders.Where(x => x.GetTotalSize() < 100000).ToList();

            return folderWithFilteredSize.Sum(x => x.GetTotalSize());
        }

        public object SolveTaskTwo(string[] input)
        {
            var folders = ExecuteCommands(input);
            long freeSpace = TOTAL_DISK_SIZE - folders.FirstOrDefault().GetTotalSize();
            long spaceNeeded = UPDATE_SIZE - freeSpace;

            var folderToDelete = folders
                .Select(x => x.GetTotalSize())
                .Where(x => x > spaceNeeded)
                .OrderBy(x => x)
                .FirstOrDefault();

            return folderToDelete;
        }

        private List<DirEntry> ExecuteCommands(string[] input)
        {
            Dictionary<string, DirEntry> result = new();

            string? currentFolder = null;
            foreach(var cmd in input)
            {
                if (cmd == "$ cd ..")
                {
                    currentFolder = result[currentFolder].Parent.GetPath();
                }
                else if (cmd.StartsWith("$ cd /"))
                {
                    result.Add("/", new DirEntry("/", null));
                    currentFolder = "/";
                }
                else if (cmd.StartsWith("$ cd"))
                {
                    string next = cmd[5..];
                    DirEntry parent = result[currentFolder];

                    var dir = new DirEntry(next, parent);
                    currentFolder = dir.GetPath();

                    result.Add(currentFolder, dir);
                    parent.Dirs.Add(dir);
                }
                else if (cmd.StartsWith("$ ls") || cmd.StartsWith("dir "))
                {
                    // do nothing?
                }
                else if (currentFolder != null)
                {
                    string[] split = cmd.Split(' ');
                    result[currentFolder].Files.Add(new FileEntry(split[1], long.Parse(split[0])));
                }
            }

            return result.Values.ToList();
        }

        private class DirEntry
        {
            public string Name { get; }

            public DirEntry? Parent { get; }

            public List<FileEntry> Files { get; } = new();

            public List<DirEntry> Dirs { get; } = new();

            public DirEntry(string name, DirEntry? parent)
            {
                Name = name;
                Parent = parent;
            }

            public long GetTotalSize()
            {
                long size = Dirs.Sum(x => x.GetTotalSize());
                size += Files.Sum(x => x.Size);
                return size;
            }

            public string GetPath()
            {
                if (Parent == null)
                    return Name;

                if (Parent.Name == "/")
                    return "/" + Name;

                return Parent.GetPath() + "/" + Name;
            }
        }

        private record FileEntry(string Name, long Size);
    }
}
