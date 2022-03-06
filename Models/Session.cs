namespace ScrumbledSession.Models
{
    public record Participant(string Name);
    public record Session(long SessionId, Participant[] Participants);

    public record SessionData(Session[] session);
}