using ScrumbledSession.Models;
using System.Text.Json;

namespace ScrumbledSession.Services
{
    public class SessionDataLoader : ISessionDataLoader
    {
        public readonly IHostEnvironment _environment;
        public SessionDataLoader(IHostEnvironment environment)
        {
            _environment = environment;
        }
        public bool SessionDataExists()
        {
            return File.Exists(GetDataFilePath());
        }

        public async Task<SessionData> Create()
        {
            var sessionData = new SessionData(new Session[] {});
            return await Create(sessionData);
        }

        public async Task<SessionData> Create(SessionData sessionData)
        {
            string fileName = GetDataFilePath();
            string jsonString = JsonSerializer.Serialize(sessionData);
            await File.WriteAllTextAsync(fileName, jsonString);
            return sessionData;
        }

        public async Task<SessionData> GetSessionData()
        {
            using FileStream fileStream = File.OpenRead(GetDataFilePath());
            return await JsonSerializer.DeserializeAsync<SessionData>(fileStream);
        }

        public string GetDataFilePath()
        {
            var dataFilePath = Path.Combine(_environment.ContentRootPath, "App_Data", "session-data.json");
            return dataFilePath;
        }
    }
}
