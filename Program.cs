using ScrumbledSession.Models;
using ScrumbledSession.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISessionService, SessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/Session", (ISessionService sessionService) =>
{
    var session = sessionService.CreateSession();
    return Results.CreatedAtRoute(value: session);
});

app.MapPut("/Session", (string sessionId, AddParticipantRequest request, ISessionService sessionService) =>
{
    sessionService.AddParticipant(request, long.Parse(sessionId));
    return Results.NoContent();
});

app.Run();