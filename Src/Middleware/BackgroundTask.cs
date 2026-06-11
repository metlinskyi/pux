using System.Text.Json;

public abstract class BackgroundTask
{
    public abstract bool IsCompleted();
    public abstract string JsonResult();
}

public class BackgroundTask<T>(Task<T> task) : BackgroundTask
{
    public override bool IsCompleted()
    {
        return task.IsCompleted;
    }
    public override string JsonResult()
    {
        return JsonSerializer.Serialize<T>(task.Result);
    }
}