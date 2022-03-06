using ScrumbledSession.Models;

namespace ScrumbledSession.Services
{
    public interface ISessionDataLoader
    {
        string GetDataFilePath();
        bool SessionDataExists();
        Task<SessionData> GetSessionData();
        Task<SessionData> Create();
        Task<SessionData> Create(SessionData sessionData);
    }
}
