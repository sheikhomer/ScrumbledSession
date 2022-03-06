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
            var sessions = data.Sessions.ToList();
            long newSessionId = sessions.Any() 
                ? sessions[sessions.Count - 1].SessionId + 1 : 555555;
            var newSession = new Session(newSessionId, new Participant[] { });
            sessions.Add(newSession);
            await _sessionDataLoader.Create(data with { Sessions = sessions.ToArray()});
            return newSession;
        }

        public async Task<Session> AddParticipant(AddParticipantRequest request, long sessionId) 
        {
            var data = await _sessionDataLoader.GetSessionData();
            var sessions = data.Sessions.ToList();
            var selectedSession = sessions.FirstOrDefault(x => x.SessionId == sessionId);
            var participants = selectedSession.Participants.ToList();
            participants.Add(new Participant( Name: request.Name ));
            var updatedSession = selectedSession with{ Participants = participants.ToArray()};
            sessions[sessions.IndexOf(selectedSession)] = updatedSession;
            await _sessionDataLoader.Create(data with { Sessions = sessions.ToArray() });
            return selectedSession;
        }

        public async Task<Participant[]> GetParticipants(long sessionId)
        {
            var data = await _sessionDataLoader.GetSessionData();
            var session = data?.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
            return session.Participants.ToArray();
        }
    }
}
