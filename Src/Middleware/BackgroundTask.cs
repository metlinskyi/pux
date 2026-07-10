using System.Text.Json;

public abstract class BackgroundTask
{
    public abstract BackgroundTaskProgress Progress {get;}
    public abstract Task Task {get;}
}

public class BackgroundTask<T>(Task<T> task) : BackgroundTask
{
    public override BackgroundTaskProgress Progress => 
    task.IsCompletedSuccessfully 
        ? new BackgroundTaskProgress{ Value = 100, Message = "Completed" }
        : new BackgroundTaskProgress{ Value = 0, Message = "In Progress" };
    public override Task Task => task;

    public override string ToString()
    {
        if(!task.IsCompletedSuccessfully)
            return JsonSerializer.Serialize(new BackgroundTaskResult<string>
            {
                Data = task.Exception?.Message
            });

        return JsonSerializer.Serialize(new BackgroundTaskResult<T>
        {
            IsCompletedSuccessfully = true,
            Data = task.Result
        });
    }
}