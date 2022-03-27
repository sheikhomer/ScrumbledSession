namespace ScrumbledSession.Models
{
    public class Participant
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public bool IsOwner { get; set; }

    }
    public class Session
    {
        public long SessionId { get; set; }
        public IList<Participant> Participants { get; set; }
    }

    public class SessionData
    {
        public IList<Session> Sessions { get; set;}
        public long UnixTimeSecond { get; set; }
    }
}