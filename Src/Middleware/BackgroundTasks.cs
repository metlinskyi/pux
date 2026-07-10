using System.Collections.Concurrent;

internal class BackgroundTasks : ConcurrentDictionary<Guid, BackgroundTask>, IBackgroundTasks
{
    public bool TryGet(Guid id, out BackgroundTask task)
    {
        return TryGetValue(id, out task!);
    }

    public bool TryRemove(Guid id)
    {
        return TryRemove(id, out _);
    }
}
