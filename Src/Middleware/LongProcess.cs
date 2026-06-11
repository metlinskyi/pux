using System.Collections.Concurrent;
using System.Text.Json;

internal class LongProcess<T>(IHttpContextAccessor accessor) : ILongProcess<T>
{
    private TimeSpan delay = TimeSpan.FromMilliseconds(500);

    private readonly ConcurrentDictionary<Guid, Task<T>> tasks = new();

    public Guid Allocate(Task<T> task)
    {
        Guid id = Guid.NewGuid();
        tasks.TryAdd(id, task);

        return id;
    }

    public async Task<T> WaitForAsync(Guid id, CancellationToken token)
    {       
        var context = accessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");

        var task = Get(id);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/json";
        context.Response.Headers.Append("Content-Type", "text/event-stream");
        context.Response.Headers.Append("Cache-Control", "no-cache");
        
        while (task.IsCompleted == false)
        {
            await context.Response.WriteAsync(" ");
            await context.Response.Body.FlushAsync();
            await Task.Delay(delay);
        }

        string json = JsonSerializer.Serialize<T>(task.Result);
        await context.Response.WriteAsync(json);

        tasks.TryRemove(id, out _);

        return task.Result;
    }

    private Task<T> Get(Guid id)
    {
        if (tasks.TryGetValue(id, out Task<T>? task))
        {
            return task;
        }

        throw new KeyNotFoundException($"Task with id {id} not found.");
    }

}