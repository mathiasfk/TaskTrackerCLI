using Core.Models;
using Core.UseCases;

namespace TaskTrackerCLI
{
    public class Driver
    {
        private readonly ICommandProcessor _commandProcessor;

        public Driver(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }
        public async Task RunAsync(
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello, internet!");
            await _commandProcessor.ProcessCommandAsync(new Command() { Description = "Hello World"});
        }
    }
}
