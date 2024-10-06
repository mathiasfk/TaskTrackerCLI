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
                case CommandVerb.Update:
                    await UpdateCommand(command);
                    break;
                case CommandVerb.List:
                    await ListCommand(command);
                    break;
                case CommandVerb.Delete:
                    await DeleteCommand(command);
                    break;
                case CommandVerb.Mark:
                    await MarkCommand(command);
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

        private async Task UpdateCommand(Command command)
        {
            if (command.Id == null)
            {
                throw new ArgumentException("Command Id cannot be null for update operation");
            }

            if (command.Description == null)
            {
                throw new ArgumentException("Description cannot be null for update operation");
            }

            var item = await _persistencePort.GetByID(command.Id.Value);
            if (item == null)
            {
                throw new InvalidOperationException("TaskItem not found");
            }

            item.Description = command.Description;

            await _persistencePort.Update(item);
        }

        private async Task ListCommand(Command command)
        {

            var list = await _persistencePort.GetAll();
            if(command.Status.HasValue)
            {
                list = list.Where(x => x.Status == command.Status).ToList();
            }
            await _outputPort.SendList(list);
        }

        private async Task DeleteCommand(Command command)
        {
            if (command.Id == null)
            {
                throw new ArgumentException("Command Id cannot be null for delete operation");
            }

            var item = await _persistencePort.GetByID(command.Id.Value);
            if (item == null)
            {
                throw new InvalidOperationException("TaskItem not found");
            }
            await _persistencePort.Remove(item);
        }

        private async Task MarkCommand(Command command)
        {
            if (command.Id == null)
            {
                throw new ArgumentException("Command Id cannot be null for mark operation");
            }

            if (command.Status == null)
            {
                throw new ArgumentException("Status cannot be null for mark operation");
            }

            var item = await _persistencePort.GetByID(command.Id.Value);
            if (item == null)
            {
                throw new InvalidOperationException("TaskItem not found");
            }

            item.Status = command.Status.Value;

            await _persistencePort.Update(item);
        }

    }
}
