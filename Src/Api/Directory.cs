using Microsoft.AspNetCore.Mvc;

public static class Directory
{
    public static void MapDirectoryEndpoints(this WebApplication app)
    {
        app.MapPost("/directory/diff/async", async (
            [FromBody]DirectoryRecord request, 
            [FromServices]IDirectoryDiffService service,
            [FromServices]IBackgroundProcess background,
            CancellationToken ct) =>
        {
            DirectoryInfo directory = new(request.Path);
            if(directory.Exists == false)
               return Results.NotFound();

            var id = background.Allocate(service.GetDirectoryDiffAsync(directory, ct));
            
            return Results.RedirectToRoute("GetBackgroundResult", new { id });
        });

        app.MapPost("/directory/diff/sync", async (
            [FromBody]DirectoryRecord request, 
            [FromServices]IDirectoryDiffService service,
            CancellationToken ct) =>
        {
            DirectoryInfo directory = new(request.Path);
            if(directory.Exists == false)
               return Results.NotFound();
            
            return Results.Ok(await service.GetDirectoryDiffAsync(directory, ct));
        });
    }
}