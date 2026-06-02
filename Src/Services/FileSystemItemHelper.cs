public static class DiffHelper
{
    public static IEnumerable<FileSystemItem> GetNewFileSystemItem(IEnumerable<FileSystemItem> prev, IEnumerable<FileSystemItem> current)
    {
        return current.Except(prev) ;
    }

    public static IEnumerable<FileSystemItem> GetDeletedFileSystemItem(IEnumerable<FileSystemItem> prev, IEnumerable<FileSystemItem> current)
    {
        return prev.Except(current);
    }

    public static IEnumerable<FileSystemItem> GetModifiedFileSystemItem(IEnumerable<FileSystemItem> prev, IEnumerable<FileSystemItem> current)
    {
        return current
            .Select(i => new { Current = i, Previous = prev.SingleOrDefault(p => p == i) })
            .Where(x => x.Previous != null && (x.Current.Size != x.Previous.Size))
            .Select(x =>
            {  
                x.Current.Version = x.Previous!.Version + 1;
                return x.Current;
            });
    }
}