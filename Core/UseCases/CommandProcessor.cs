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

        public async Task ProcessCommand(Command command)
        {
            var list = new List<TaskItem>();
            var item = new TaskItem {
                Id = 123,
                Description = "Buy groceries",
                Status = Status.ToDo
            };
            list.Add(item);

            await _outputPort.SendList(list);
            await _persistencePort.Add(item);
        }
    }
}
