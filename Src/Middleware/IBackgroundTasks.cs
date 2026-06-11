public interface IBackgroundTasks
{
    public bool TryAdd(Guid id, BackgroundTask task);
    public bool TryGet(Guid id, out BackgroundTask task);
    public bool TryRemove(Guid id);
}