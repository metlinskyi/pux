using System.Text.Json;

public class BackgroundTaskProgress
{
    public int Value {get; set;}
    public string? Message {get; set;}
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
