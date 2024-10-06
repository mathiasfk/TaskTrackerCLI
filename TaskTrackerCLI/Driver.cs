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
            Command command;
            switch (args[0])
            {
                case CMD_ADD:
                    command = new()
                    {
                        Verb = CommandVerb.Add,
                        Description = args[1],
                        Status = Status.ToDo
                    };
                    break;

                case CMD_UPDATE:
                    command = new()
                    {
                        Verb = CommandVerb.Update,
                        Id = int.Parse(args[1]),
                        Description = args[2]
                    };
                    break;

                case CMD_DELETE:
                    command = new()
                    { 
                        Verb = CommandVerb.Delete,
                        Id = int.Parse(args[1])
                    };
                    break;

                case CMD_LIST:
                    command = new()
                    {
                        Verb = CommandVerb.List,
                    };
                    if (args.Length > 1)
                    {
                        command.Status = ParseStatus(args[1]);
                    }
                    break;

                case CMD_MARK_IN_PROGRESS:
                    command = new()
                    {
                        Verb = CommandVerb.Mark,
                        Id = int.Parse(args[1]),
                        Status = Status.InProgress
                    };
                    break;

                case CMD_MARK_DONE:
                    command = new()
                    {
                        Verb = CommandVerb.Mark,
                        Id = int.Parse(args[1]),
                        Status = Status.Done
                    };
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