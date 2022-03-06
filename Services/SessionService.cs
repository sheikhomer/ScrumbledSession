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
        public async Task<Session> CreateSession()
        {
            var data = _sessionDataLoader.SessionDataExists()
                ? await _sessionDataLoader.GetSessionData() : await _sessionDataLoader.Create();
            var sessions = data.session;
            long newSessionId = sessions.Any() 
                ? sessions[sessions.Length - 1].SessionId + 1 : 555555;
            var newSession = new Session(newSessionId, new Participant[] { });
            sessions.Append(newSession);
            await _sessionDataLoader.Create(data);
            return newSession;
        }

        public async Task<Session> AddParticipant(AddParticipantRequest request, long sessionId) 
        {
            var data = await _sessionDataLoader.GetSessionData();
            var session = data?.session.FirstOrDefault(x => x.SessionId == sessionId);
            session.Participants.Append(new Participant( Name: request.Name ));
            return session;
        }

        public async Task<Participant[]> GetParticipants(long sessionId)
        {
            var data = await _sessionDataLoader.GetSessionData();
            var session = data?.session.FirstOrDefault(x => x.SessionId == sessionId);
            return session.Participants.ToArray();
        }
    }
}
