using System.Collections.Concurrent;
using static DiffHelper;

internal class DirectoryDiffService(ISnapshotService snapshotService) : IDirectoryDiffService
{
    private readonly ConcurrentDictionary<string, FileSystemItem[]> _cache = new ();

    public async Task<DirectoryDiff?> GetDirectoryDiffAsync(DirectoryInfo directory)
    {        
        // Emulation of long process
        await Task.Delay(TimeSpan.FromSeconds(10));

        DirectoryDiff? diff = new ();

        var currentSnapshot = snapshotService.From(directory).ToArray();
        if (_cache.TryGetValue(directory.FullName, out var prevSnapshot))
        {
            CreateDiff(diff, prevSnapshot, currentSnapshot);
        }

        _cache[directory.FullName] = currentSnapshot;

        return await Task.FromResult<DirectoryDiff?>(diff);
    }

    private void CreateDiff(DirectoryDiff diff, FileSystemItem[] prevSnapshot, FileSystemItem[] currentSnapshot)
    {
        var newItems = GetNewFileSystemItem(prevSnapshot, currentSnapshot).ToArray();
        var deletedItems = GetDeletedFileSystemItem(prevSnapshot, currentSnapshot).ToArray();

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

        diff.ModifiedFiles = GetModifiedFileSystemItem(prevSnapshot, currentSnapshot)
            .Where(i => i.Type == FileSystemType.File)
            .Select(i => new FileDto { FullName = i.FullName, Version = i.Version })
            .ToArray();
    }
}