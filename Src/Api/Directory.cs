using Microsoft.AspNetCore.Mvc;

public static class Directory
{
    public static void MapDirectoryEndpoints(this WebApplication app)
    {
        app.MapPost("/directory/diff/async", async (
            [FromBody]DirectoryRecord request, 
            [FromServices]IDirectoryDiffService directoryDiff,
            [FromServices]IBackgroundProcess background,
            CancellationToken ct) =>
        {
            DirectoryInfo directory = new(request.Path);
            if(directory.Exists == false)
               return Results.NotFound();

            var id = background.Allocate(directoryDiff.GetDirectoryDiffAsync(directory, ct));
            
            return Results.RedirectToRoute("GetBackgroundResult", new { id });
        });

        app.MapPost("/directory/diff/sync", async (
            [FromBody]DirectoryRecord request, 
            [FromServices]IDirectoryDiffService directoryDiff,
            CancellationToken ct) =>
        {
            DirectoryInfo directory = new(request.Path);
            if(directory.Exists == false)
               return Results.NotFound();
            
            return Results.Ok(await directoryDiff.GetDirectoryDiffAsync(directory, ct));
        });
    }
}