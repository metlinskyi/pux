internal class SnapshotService : ISnapshotService
{
    public IEnumerable<FileSystemItem> From(DirectoryInfo directory)
    {
        foreach (var file in directory.GetFiles())
        {
            yield return new FileSystemItem
            {
                Type = FileSystemType.File,
                FullName = file.FullName,
                Size = (int)file.Length
            };
        }

        foreach (var subDirectory in directory.GetDirectories())
        {
            foreach (var item in From(subDirectory))
            {
                yield return item;
            }
            
            yield return new FileSystemItem
            {
                Type = FileSystemType.Directory,
                FullName = subDirectory.FullName,
                Size = 0
            };
        }
    }
}