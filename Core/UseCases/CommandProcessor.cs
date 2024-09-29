using Core.Models;
using Core.Ports;

namespace Core.UseCases
{
    public class CommandProcessor: ICommandProcessor
    {
        private IPersistencePort _persistencePort;
        private IOutputPort _outputPort;

        public CommandProcessor(IPersistencePort persistencePort, IOutputPort outputPort)
        {
            _persistencePort = persistencePort;
            _outputPort = outputPort;
        }

        public async Task ProcessCommandAsync(Command command)
        {
            var item = new TaskItem {
                Id = 123,
                Description = command.Description ?? "",
                Status = Status.ToDo
            };

            await _persistencePort.Add(item);
        }
    }
}
