public interface IDirectoryDiffService
{
    Task<DirectoryDiff> GetDirectoryDiffAsync(DirectoryInfo directory, CancellationToken token);
}