using Microsoft.AspNetCore.Http.Timeouts;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
    Args = args,
    WebRootPath = "../Web/"
});
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IBackgroundProcess, BackgroundProcess>();
builder.Services.AddSingleton<IDirectoryDiffService, DirectoryDiffService>();
builder.Services.AddTransient<ISnapshotService, SnapshotService>();
builder.Services.AddCors(___ => {
    ___.AddPolicy(name: "*",
                      policy  => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()   
                    );
});
builder.Services.AddRequestTimeouts(options => {
    options.DefaultPolicy = new RequestTimeoutPolicy { 
        Timeout = TimeSpan.FromSeconds(2) 
    };
});
var app = builder.Build();
app.UseRequestTimeouts();
app.UseCors("*");
app.UseDefaultFiles(new DefaultFilesOptions {
    DefaultFileNames = ["index.html"]
});
app.UseStaticFiles();
app.MapOpenApi();
app.MapDirectoryEndpoints();
app.MapBackgroundEndpoints();
app.Run();
