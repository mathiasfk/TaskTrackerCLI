using Core.Models;
using Core.Ports;
using Core.UseCases;
using NSubstitute;

namespace Core.Test.UseCases
{
    [TestClass]
    public class CommandProcessorTests
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IOutputPort _outputPort;
        private readonly IPersistencePort _persistencePort;


        public CommandProcessorTests()
        {
            _outputPort = Substitute.For<IOutputPort>();
            _persistencePort = Substitute.For<IPersistencePort>();
            _commandProcessor = new CommandProcessor(_persistencePort, _outputPort);
        }

        [TestMethod]
        public async Task AddItem_WithDescription_ShouldSucceedAsync()
        {
            // Arrange
            var description = "Test item";
            var command = new Command()
            {
                Verb = CommandVerb.Add,
                Description = description
            };

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Add(Arg.Is<TaskItem>(item => item.Description == description));
        }
    }
}
