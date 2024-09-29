﻿using Core.Models;
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

        [TestMethod]
        public async Task AddItem_WithoutDescription_ShouldSucceedAsync()
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
        public async Task AddItem_WithoutPreviousHistory_ShouldReceiveIdOneAsync()
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
        public async Task AddItem_WithPreviousHistory_ShouldReceiveHigherIdAsync()
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
        public async Task AddItem_WithPreviousNegativeId_ShouldReceiveIdOneAsync()
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
    }
}
