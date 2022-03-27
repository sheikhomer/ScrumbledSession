using ScrumbledSession.Models;

namespace ScrumbledSession.Services
{
    public interface ISessionService
    {
        Task<Session> CreateSession();
        Task<Session> AddParticipant(ParticipantRequest request, long sessionId);
        Task<IList<Participant>> GetParticipants(long sessionId);
        Task<Session> GetSession(long sessionId);
        Task<Session> UpdateParticipant(ParticipantRequest request, long sessionId);
    }
}
