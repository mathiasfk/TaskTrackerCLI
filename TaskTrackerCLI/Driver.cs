using Core.Models;
using Core.UseCases;

namespace TaskTrackerHost.CLI
{
    public class Driver(ICommandProcessor commandProcessor, string[] args)
    {
        private readonly ICommandProcessor _commandProcessor = commandProcessor;
        private readonly string[] _args = args;

        private const string CMD_ADD = "add";
        private const string CMD_UPDATE = "update";
        private const string CMD_DELETE = "delete";
        private const string CMD_MARK_IN_PROGRESS = "mark-in-progress";
        private const string CMD_MARK_DONE = "mark-done";
        private const string CMD_LIST = "list";

        public async Task RunAsync(
            CancellationToken cancellationToken)
        {
            var command = ParseArguments(_args);
            await _commandProcessor.ProcessCommandAsync(command);
        }

        private Command ParseArguments(string[] args)
        {
            var command = new Command();
            switch (args[0])
            {
                case CMD_ADD:
                    command.Verb = CommandVerb.Add;
                    command.Description = args[1];
                    command.Status = Status.ToDo;
                    break;

                case CMD_UPDATE:
                    command.Verb = CommandVerb.Update;
                    command.Id = int.Parse(args[1]);
                    command.Description = args[2];
                    break;

                case CMD_DELETE:
                    command.Verb = CommandVerb.Delete;
                    command.Id = int.Parse(args[1]);
                    break;

                case CMD_LIST:
                    command.Verb = CommandVerb.List;
                    if (args.Length > 1)
                    {
                        command.Status = ParseStatus(args[1]);
                    }
                    break;

                case CMD_MARK_IN_PROGRESS:
                    command.Id = int.Parse(args[1]);
                    command.Status = Status.InProgress;
                    break;

                case CMD_MARK_DONE:
                    command.Id = int.Parse(args[1]);
                    command.Status = Status.Done;
                    break;

                default:
                    throw new ArgumentException("Invalid command: {command}", args[0]);
            }
            return command;
        }

        private static Status ParseStatus(string status)
        {
            return status.ToLower() switch
            {
                "todo" => Status.ToDo,
                "in-progress" => Status.InProgress,
                "done" => Status.Done,
                _ => Status.ToDo,
            };
        }
    }
}