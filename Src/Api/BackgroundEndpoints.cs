using Microsoft.AspNetCore.Mvc;

public static class BackgroundEndpoints
{
    public static void MapBackgroundEndpoints(this WebApplication app)
    {
        app.MapGet("/background/result/{id}", async (
            [FromRoute]Guid id, 
            [FromServices]IBackgroundProcess background,
            CancellationToken ct) =>
        {
            await background.WaitForAsync(id, ct);
        });
    }
}