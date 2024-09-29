using Core.Models;

namespace Core.Ports
{
    public interface IPersistencePort
    {
        public Task<TaskItem?> GetByID(int id);

        public Task<int> GetLatestID();

        public Task Add(TaskItem item);

        public Task Remove(TaskItem item);

        public Task Update(TaskItem item);
    }
}
