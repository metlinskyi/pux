public interface IBackgroundProcess
{
    Guid Allocate<T>(Task<T> task);
    Task WaitForAsync(Guid id, CancellationToken token);
}