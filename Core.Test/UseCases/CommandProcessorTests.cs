using Core.Models;
using Core.Ports;
using Core.UseCases;
using NSubstitute;

namespace Core.Test.UseCases
{
    [TestClass]
    public class CommandProcessorTests
    {
        private readonly CommandProcessor _commandProcessor;
        private readonly IOutputPort _outputPort;
        private readonly IPersistencePort _persistencePort;


        public CommandProcessorTests()
        {
            _outputPort = Substitute.For<IOutputPort>();
            _persistencePort = Substitute.For<IPersistencePort>();
            _commandProcessor = new(_persistencePort, _outputPort);
        }

        [TestMethod]
        public async Task Add_WithDescription_ShouldSucceedAsync()
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

        [TestMethod]
        public async Task Add_WithoutDescription_ShouldSucceedAsync()
        {
            // Arrange
            var command = new Command()
            {
                Verb = CommandVerb.Add
            };

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Add(Arg.Is<TaskItem>(item => item.Description == string.Empty));
        }

        [TestMethod]
        public async Task Add_WithoutPreviousHistory_ShouldReceiveIdOneAsync()
        {
            // Arrange
            var command = new Command()
            {
                Verb = CommandVerb.Add
            };

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Add(Arg.Is<TaskItem>(item => item.Id == 1));
        }

        [TestMethod]
        public async Task Add_WithPreviousHistory_ShouldReceiveHigherIdAsync()
        {
            // Arrange
            var previousId = 456;
            var command = new Command()
            {
                Verb = CommandVerb.Add
            };
            _persistencePort.GetLatestID().Returns(previousId);

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Add(Arg.Is<TaskItem>(item => item.Id == previousId + 1));
        }

        [TestMethod]
        public async Task Add_WithPreviousNegativeId_ShouldReceiveIdOneAsync()
        {
            // Arrange
            var previousId = -1;
            var command = new Command()
            {
                Verb = CommandVerb.Add
            };
            _persistencePort.GetLatestID().Returns(previousId);

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Add(Arg.Is<TaskItem>(item => item.Id == 1));
        }

        [TestMethod]
        public async Task Add_WithId_ShouldIgnoreIdAsync()
        {
            // Arrange
            var command = new Command()
            {
                Verb = CommandVerb.Add,
                Id = 123
            };

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Add(Arg.Is<TaskItem>(item => item.Id == 1));
        }

        [TestMethod]
        public async Task List_WithoutHistory_ShouldSendEmptyListAsync()
        {
            // Arrange
            var command = new Command()
            {
                Verb = CommandVerb.List
            };
            _persistencePort.GetAll().Returns([]);

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _outputPort.Received(1).SendList(Arg.Is<List<TaskItem>>(list => list.Count == 0));
        }

        [TestMethod]
        public async Task List_WithHistory_ShouldSendListAsync()
        {
            // Arrange
            var expectedList = new List<TaskItem>()
            {
                new()
                {
                    Id = 1,
                    Description = "Buy groceries",
                    Status = Status.Done
                },
                new(){
                    Id = 2,
                    Description = "Clean the fridge",
                    Status = Status.ToDo
                }
            };
            var command = new Command()
            {
                Verb = CommandVerb.List
            };
            _persistencePort.GetAll().Returns(expectedList);

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _outputPort.Received(1).SendList(Arg.Is<List<TaskItem>>(list => list == expectedList));
        }

        [TestMethod]
        public async Task List_WithFilter_ShouldSendFilteredListAsync()
        {
            // Arrange
            var expectedList = new List<TaskItem>()
            {
                new()
                {
                    Id = 1,
                    Description = "Buy groceries",
                    Status = Status.Done
                },
                new(){
                    Id = 2,
                    Description = "Clean the fridge",
                    Status = Status.ToDo
                }
            };
            var command = new Command()
            {
                Verb = CommandVerb.List,
                Status = Status.Done
            };
            _persistencePort.GetAll().Returns(expectedList);

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _outputPort.Received(1).SendList(Arg.Is<List<TaskItem>>(list => list.Count == 1));
        }

        [TestMethod]
        public async Task Update_WithItemSaved_ShouldSucceedAsync()
        {
            // Arrange
            var id = 123;
            var oldDescription = "Old description";
            var newDescription = "New description";

            _persistencePort.GetByID(Arg.Any<int>()).Returns(new TaskItem() { 
                Id = id,
                Description = oldDescription,
                Status = Status.ToDo
            });

            var command = new Command()
            {
                Verb = CommandVerb.Update,
                Description = newDescription,
                Id = id
            };

            // Act
            await _commandProcessor.ProcessCommandAsync(command);

            // Assert
            _ = _persistencePort.Received(1).Update(Arg.Is<TaskItem>(item => item.Description == newDescription));
        }

        [TestMethod]
        public async Task Update_WithUnexistantItem_ShouldFailAsync()
        {
            // Arrange
            var id = 123;
            var newDescription = "New description";

            var command = new Command()
            {
                Verb = CommandVerb.Update,
                Description = newDescription,
                Id = id
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await _commandProcessor.ProcessCommandAsync(command)
            );
        }

        [TestMethod]
        public async Task Update_WithoutDescription_ShouldFailAsync()
        {
            // Arrange
            var id = 123;
            var command = new Command()
            {
                Verb = CommandVerb.Update,
                Id = id
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _commandProcessor.ProcessCommandAsync(command)
            );
        }

        [TestMethod]
        public async Task Update_WithoutId_ShouldFailAsync()
        {
            // Arrange
            var command = new Command()
            {
                Verb = CommandVerb.Update,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _commandProcessor.ProcessCommandAsync(command)
            );
        }
    }
}
