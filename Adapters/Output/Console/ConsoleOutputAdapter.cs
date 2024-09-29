using Core.Models;
using Core.Ports;
using Microsoft.Extensions.Logging;

namespace Adapters.Output.Console
{
    public class ConsoleOutputAdapter : IOutputPort
    {
        private readonly ILogger<ConsoleOutputAdapter> _logger;
        public ConsoleOutputAdapter(ILogger<ConsoleOutputAdapter> logger)
        {
            _logger = logger;
        }

        public Task SendList(List<TaskItem> list)
        {
            foreach (var task in list)
            {
                _logger.LogInformation("{Id} - {Description} [{Status}]", task.Id, task.Description, task.Status);
            }
            return Task.CompletedTask;
        }
    }
}
