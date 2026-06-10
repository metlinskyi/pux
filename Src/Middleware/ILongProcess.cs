public interface ILongProcess<T>
{
    Guid Allocate(Task<T> task);
    Task<T> WaitForAsync(Guid id, CancellationToken token);
}