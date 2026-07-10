internal class BackgroundProcess(
    ILogger<BackgroundProcess> logger,
    IBackgroundTasks tasks, 
    IHttpContextAccessor accessor
    ) : IBackgroundProcess
{
    private TimeSpan delay = TimeSpan.FromMilliseconds(500);
    public async Task WaitForAsync(Guid id, CancellationToken token)
    {
        var context = accessor.HttpContext 
            ?? throw new InvalidOperationException("HttpContext is not available.");

        logger.LogInformation("TryGet({id})", id);
        if (!tasks.TryGet(id, out BackgroundTask task))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.Body.FlushAsync(token);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/json";
        context.Response.Headers.Append("X-Accel-Buffering", "no");
        context.Response.Headers.Append("Content-Type", "text/event-stream");
        context.Response.Headers.Append("Cache-Control", "no-cache");

        while (task.Task.IsCompleted == false)
        {           
            await context.SendEvent("progress", task.Progress.ToString());
            await Task.Delay(delay);
        }

        await context.SendEvent("result", task.ToString());

        tasks.TryRemove(id);
    }   

    public Guid Allocate<T>(Task<T> task)
    {
        Guid id = Guid.NewGuid();
        logger.LogInformation("TryAdd({id})", id);
        if (tasks.TryAdd(id, new BackgroundTask<T>(task)))
            return id;  

        throw new Exception("Task was not allocate!");
    }
}