using Core.Models;

namespace Core.Ports
{
    public interface IOutputPort
    {
        Task SendList(List<TaskItem> list);
    }
}
