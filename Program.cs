using ScrumbledSession.Models;
using ScrumbledSession.Services;
using System.Web.Http;

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
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
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

app.MapPut("/Session/{sessionId}/user", async (string sessionId, ParticipantRequest request, ISessionService sessionService) =>
{
    await sessionService.AddParticipant(request, long.Parse(sessionId));
    return Results.NoContent();
});

app.MapPut("/Session/{sessionId}/user/{userId}", async ([FromUri]string sessionId, [FromUri] string userId, [FromBody]ParticipantRequest request, ISessionService sessionService) =>
{
    if(request.UserId != userId)
    {
        return Results.BadRequest();
    }
    await sessionService.UpdateParticipant(request, long.Parse(sessionId));
    return Results.NoContent();
});

app.Run();