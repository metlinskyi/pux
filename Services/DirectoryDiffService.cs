using System.Collections.Concurrent;

internal class DirectoryDiffService(ISnapshotService snapshotService) : IDirectoryDiffService
{
    private readonly ConcurrentDictionary<string, FileSystemItem[]> _cache = new ();

    public Task<DirectoryDiff?> GetDirectoryDiffAsync(DirectoryInfo directory)
    {        
        DirectoryDiff? diff = new ();

        var currentSnapshot = snapshotService.From(directory).ToArray();
        if (_cache.TryGetValue(directory.FullName, out var prevSnapshot))
        {
            CreateDiff(diff, prevSnapshot, currentSnapshot);
        }

        _cache[directory.FullName] = currentSnapshot;

        return Task.FromResult<DirectoryDiff?>(diff);
    }

    private void CreateDiff(DirectoryDiff diff, FileSystemItem[] prevSnapshot, FileSystemItem[] currentSnapshot)
    {
        var newItems = DiffHelper.GetNewFileSystemItem(prevSnapshot, currentSnapshot).ToArray();
        var deletedItems = DiffHelper.GetDeletedFileSystemItem(prevSnapshot, currentSnapshot).ToArray();

        diff.NewFiles = newItems
            .Where(i => i.Type == FileSystemType.File)
            .Select(i => new FileDto { FullName = i.FullName, Version = i.Version })
            .ToArray();

        diff.DeletedFiles = deletedItems
            .Where(i => i.Type == FileSystemType.File)
            .Select(i => new FileDto { FullName = i.FullName, Version = i.Version })
            .ToArray();

        diff.NewSubdirectories = newItems
            .Where(i => i.Type == FileSystemType.Directory)
            .Select(i => i.ToString())
            .ToArray();

        diff.DeletedSubdirectories = deletedItems
            .Where(i => i.Type == FileSystemType.Directory)
            .Select(i => i.ToString())
            .ToArray();

        diff.ModifiedFiles = DiffHelper.GetModifiedFileSystemItem(prevSnapshot, currentSnapshot)
            .Where(i => i.Type == FileSystemType.File)
            .Select(i => new FileDto { FullName = i.FullName, Version = i.Version })
            .ToArray();
    }
}