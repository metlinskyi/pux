using System.Collections.Concurrent;
using static DiffHelper;

internal class DirectoryDiffService(
    ISnapshotService snapshotService
    ) : ConcurrentDictionary<string, FileSystemItem[]>, IDirectoryDiffService
{
    public async Task<DirectoryDiff> GetDirectoryDiffAsync(DirectoryInfo directory, CancellationToken token)
    {    
        // Emulation of long 
        for (int i = 0; i < 10; i++)
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }

        DirectoryDiff? diff = new ();

        var currentSnapshot = snapshotService.From(directory).ToArray();
        if (TryGetValue(directory.FullName, out var prevSnapshot))
        {
            CreateDiff(diff, prevSnapshot, currentSnapshot);
        }

        this[directory.FullName] = currentSnapshot;

        return await Task.FromResult<DirectoryDiff>(diff);
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