using ScrumbledSession.Models;

namespace ScrumbledSession.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionDataLoader _sessionDataLoader;
        public SessionService(ISessionDataLoader sessionDataLoader)
        {
            _sessionDataLoader = sessionDataLoader;
        }

        public async Task<Session> GetSession(long sessionId)
        {
            var data = await _sessionDataLoader.GetSessionData();
            return data.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
        }

        public async Task<Session> CreateSession()
        {
            var data = _sessionDataLoader.SessionDataExists()
                ? await _sessionDataLoader.GetSessionData() : await _sessionDataLoader.Create();
            
            long newSessionId = data.Sessions.Any()
                ? data.Sessions[data.Sessions.Count - 1].SessionId + 1 : 555555;
            var newSession = new Session
            {
                SessionId = newSessionId,
                Participants = new List<Participant> { new Participant {
                    UserId = Guid.NewGuid().ToString(),
                    IsOwner = true,
                }},
            };
            data.Sessions.Add(newSession);
            await _sessionDataLoader.Create(data);
            return newSession;
        }

        public async Task<Session> AddParticipant(ParticipantRequest request, long sessionId) 
        {
            var data = await _sessionDataLoader.GetSessionData();
            var selectedSession = data.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
            selectedSession.Participants.Add(new Participant { Name = request.Name, UserId = Guid.NewGuid().ToString() });
            await _sessionDataLoader.Create(data);
            return selectedSession;
        }

        public async Task<Session> UpdateParticipant(ParticipantRequest request, long sessionId)
        {
            var data = await _sessionDataLoader.GetSessionData();
            var selectedSession = data.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
            var selectedParticipant = selectedSession.Participants.FirstOrDefault(x => x.UserId == request.UserId);
            selectedParticipant.Name = request.Name;
            await _sessionDataLoader.Create(data);
            return selectedSession;
        }

        public async Task<IList<Participant>> GetParticipants(long sessionId)
        {
            var data = await _sessionDataLoader.GetSessionData();
            var session = data?.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
            return session.Participants.ToList();
        }
    }
}
