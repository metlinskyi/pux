using Microsoft.AspNetCore.Mvc;

public static class Directory
{
    public static void GetDirectory(this WebApplication app)
    {
        app.MapPost("/directory/diff", async (
            [FromBody]string path, 
            ILongProcess<DirectoryDiff?> longProcess,
            IDirectoryDiffService directoryDiff) =>
        {
            DirectoryInfo directory = new(path);
            if(directory.Exists == false)
               return Results.NotFound();

            var id = longProcess.Allocate(directoryDiff.GetDirectoryDiffAsync(directory));

            return Results.RedirectToRoute("DirectoryDiffResult", new { id });
        });

        app.MapGet("/directory/diff/{id}", async (Guid id, ILongProcess<DirectoryDiff?> longProcess) =>
        {
            await longProcess.WaitForAsync(id);
        }).WithName("DirectoryDiffResult");
    }
}