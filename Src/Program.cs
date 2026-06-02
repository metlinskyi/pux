var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(typeof(ILongProcess<>), typeof(LongProcess<>));
builder.Services.AddSingleton<IDirectoryDiffService, DirectoryDiffService>();
builder.Services.AddTransient<ISnapshotService, SnapshotService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDirectoryEndpoints();
app.Run();
