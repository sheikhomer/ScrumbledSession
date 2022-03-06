using ScrumbledSession.Models;

namespace ScrumbledSession.Services
{
    public interface ISessionService
    {
        Task<Session> CreateSession();
        Task<Session> AddParticipant(AddParticipantRequest request, long sessionId);
        Task<Participant[]> GetParticipants(long sessionId);
    }
}
