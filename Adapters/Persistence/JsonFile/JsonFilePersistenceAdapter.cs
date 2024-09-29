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
            _tasks = LoadFromFile();
        }

        public async Task<int> GetLatestID()
        {
            if (_tasks.Count == 0)
            {
                return -1;
            }
            return await Task.Run(() => _tasks.Max(r => r.Id));
        }

        public async Task<TaskItem?> GetByID(int id)
        {
            var jsonItem = await Task.Run(() => _tasks.FirstOrDefault(r => r.Id == id));
            return (TaskItem?)jsonItem;
        }

        public async Task Add(TaskItem item)
        {
            var jsonItem = (JsonFileTaskItem?)item;
            if(jsonItem != null)
            {
                _tasks.Add(jsonItem);
                await Task.Run(() => StoreIntoFile());
            }
        }

        public Task<List<TaskItem>> GetAll()
        {
            var tasks = new List<TaskItem>();
            foreach (var jsonTask in _tasks)
            {
                var task = (TaskItem?)jsonTask;
                if(task != null)
                    tasks.Add(task);
            }
            return Task.FromResult(tasks);
        }

        public Task<List<TaskItem>> GetFilteredByStatus(Status filter)
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

        private List<JsonFileTaskItem> LoadFromFile()
        {
            List<JsonFileTaskItem>? list = null;
            if (File.Exists(_fileName))
            {
                try
                {
                    string jsonString = File.ReadAllText(_fileName);
                    list = JsonSerializer.Deserialize<List<JsonFileTaskItem>>(jsonString);
                }
                catch (Exception)
                {
                }
            }
            return list ?? [];
        }

        private void StoreIntoFile()
        {
            string outputString = JsonSerializer.Serialize(_tasks);
            File.WriteAllText(_fileName, outputString);
        }
    }
}
