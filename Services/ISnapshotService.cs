public interface ISnapshotService
{
    IEnumerable<FileSystemItem> From(DirectoryInfo directory);
}