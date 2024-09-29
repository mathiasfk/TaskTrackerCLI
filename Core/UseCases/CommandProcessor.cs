using Core.Models;
using Core.Ports;

namespace Core.UseCases
{
    public class CommandProcessor : ICommandProcessor
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
            switch (command.Verb)
            {
                case CommandVerb.Add:
                    await AddCommand(command);
                    break;
                case CommandVerb.List:
                    await ListCommand(command);
                    break;
                default:
                    throw new NotImplementedException("This command is not implemented");
            }
        }

        private async Task AddCommand(Command command)
        {
            var maxId = await _persistencePort.GetLatestID();
            if (maxId <= 0)
            {
                maxId = 0;
            }

            var item = new TaskItem
            {
                Id = maxId + 1,
                Description = command.Description ?? "",
                Status = Status.ToDo
            };

            await _persistencePort.Add(item);
        }

        private async Task ListCommand(Command command)
        {
            var list = await _persistencePort.GetAll();
            await _outputPort.SendList(list);
        }
    }
}
