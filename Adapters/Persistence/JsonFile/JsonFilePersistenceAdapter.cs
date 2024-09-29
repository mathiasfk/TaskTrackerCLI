using Core.Models;
using Core.Ports;
using System.Text.Json;

namespace Adapters.Persistence.JsonFile
{
    public class JsonFilePersistenceAdapter : IPersistencePort
    {
        private readonly List<JsonFileTaskItem> _tasks;
        private readonly string _fileName = "json-file-persistence.json";

        public JsonFilePersistenceAdapter()
        {
            _tasks = LoadFromFile(_fileName);
        }

        public async Task<TaskItem?> GetByID(int id)
        {
            var jsonItem = await Task.Run(() => _tasks.FirstOrDefault(r => r.Id == id));
            return (TaskItem?)jsonItem;
        }

        public Task Add(TaskItem item)
        {
            throw new NotImplementedException();
        }

        public Task Remove(TaskItem item)
        {
            throw new NotImplementedException();
        }

        public Task Update(TaskItem item)
        {
            throw new NotImplementedException();
        }

        private static List<JsonFileTaskItem> LoadFromFile(string filename)
        {
            List<JsonFileTaskItem>? list = null;
            if (File.Exists(filename))
            {
                try
                {
                    string jsonString = File.ReadAllText(filename);
                    list = JsonSerializer.Deserialize<List<JsonFileTaskItem>>(jsonString);
                }
                catch (Exception)
                {
                }
            }
            return list ?? [];
        }
    }
}
