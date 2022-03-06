using ScrumbledSession.Models;
using ScrumbledSession.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISessionDataLoader, SessionDataLoader>();
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapGet("/Session/{sessionId}", async (string sessionId, ISessionService sessionService) =>
{
    long sessionValue;
    if(!long.TryParse(sessionId, out sessionValue))
    {
        return Results.BadRequest();
    }
    var session = await sessionService.GetSession(sessionValue);
    return Results.Ok(session);
});

app.MapPost("/Session", async (ISessionService sessionService) =>
{
    var session = await sessionService.CreateSession();
    return Results.Ok(session);
});

app.MapPut("/Session", async (string sessionId, AddParticipantRequest request, ISessionService sessionService) =>
{
    await sessionService.AddParticipant(request, long.Parse(sessionId));
    return Results.NoContent();
});

app.Run();