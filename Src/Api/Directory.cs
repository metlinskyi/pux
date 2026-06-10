using Microsoft.AspNetCore.Mvc;

public static class Directory
{
    public static void MapDirectoryEndpoints(this WebApplication app)
    {
        app.MapPost("/directory/diff/async", async (
            [FromBody]DirectoryRecord request, 
            ILongProcess<DirectoryDiff?> longProcess,
            IDirectoryDiffService directoryDiff) =>
        {
            DirectoryInfo directory = new(request.Path);
            if(directory.Exists == false)
               return Results.NotFound();

            var id = longProcess.Allocate(directoryDiff.GetDirectoryDiffAsync(directory));

            return Results.RedirectToRoute("DirectoryDiffResult", new { id });
        });

        app.MapGet("/directory/diff/{id}", async (
            Guid id, 
            ILongProcess<DirectoryDiff?> longProcess) =>
        {
            await longProcess.WaitForAsync(id);
        })
        .WithName("DirectoryDiffResult");

        app.MapPost("/directory/diff/sync", async (
            [FromBody]DirectoryRecord request, 
            IDirectoryDiffService directoryDiff) =>
        {
            DirectoryInfo directory = new(request.Path);
            if(directory.Exists == false)
               return Results.NotFound();

            return Results.Ok(await directoryDiff.GetDirectoryDiffAsync(directory));
        });
    }
}