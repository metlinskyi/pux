public static class EventSourceHelper
{
    public static async Task SendEvent(this HttpContext context, string @event, string? data)
    {
        await context.Response.WriteAsync("event: ");
        await context.Response.WriteAsync(@event);  
        await context.Response.WriteAsync("\n");  
        await context.Response.WriteAsync("data: ");
        await context.Response.WriteAsync(data??"");  
        await context.Response.WriteAsync("\n\n");  
        await context.Response.Body.FlushAsync();
    }
}