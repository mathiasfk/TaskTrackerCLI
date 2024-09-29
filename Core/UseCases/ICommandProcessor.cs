using Core.Models;

namespace Core.UseCases
{
    public interface ICommandProcessor
    {
        Task ProcessCommandAsync(Command command);
    }
}
