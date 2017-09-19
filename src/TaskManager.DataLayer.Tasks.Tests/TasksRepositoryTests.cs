using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using TaskManager.DataLayer.Repositories;
using TaskManager.DataLayer.Tasks.Entities;
using TaskManager.Domain.Tasks.Models;
using TaskManager.Universal.DateTime.DateTimeProvider;
using Xunit;

namespace TaskManager.DataLayer.Tasks.Tests
{
    public class TasksRepositoryTests
    {
        private readonly Mock<IGuidFactory> _guidFactory;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly Mock<IDbTmRepository<DbTask>> _dbRepository;
        private readonly TasksRepository _tasksRepository;
        private readonly Fixture _fixture;

        public TasksRepositoryTests()
        {
            _guidFactory = new Mock<IGuidFactory>(MockBehavior.Strict);
            _dateTimeProvider = new Mock<IDateTimeProvider>(MockBehavior.Strict);
            _dbRepository = new Mock<IDbTmRepository<DbTask>>(MockBehavior.Strict);
            _tasksRepository = new TasksRepository(_guidFactory.Object,_dateTimeProvider.Object, _dbRepository.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async void AddAsync_NewTaskAdded()
        {
            var newTask = _fixture.Create<TaskDomain>();
            var newGuid = _fixture.Create<Guid>();
            var newDateTime = _fixture.Create<DateTime>();

            var dbTask = new DbTask
            {
                Id = newGuid,
                AddedDate = newDateTime,
                State = newTask.State,
                DueDate = newTask.DueDate,
                Priority = newTask.Priority,
                Description = newTask.Description
            };

            _guidFactory.Setup(x=>x.Create()).Returns(newGuid).Verifiable();
            _dateTimeProvider.Setup(x=>x.Now).Returns(newDateTime).Verifiable();
            _dbRepository
                .Setup(x => x.Add(It.IsAny<DbTask>()))
                .Callback<DbTask>(x => x.Should().BeEquivalentTo(dbTask))
                .Verifiable();

            await _tasksRepository.AddAsync(newTask);

            _dateTimeProvider.VerifyAll();
            _dbRepository.VerifyAll();
            _guidFactory.VerifyAll();
        }

        [Fact]
        public async void GetAsync_ReturnsTask()
        {
            var newGuid = _fixture.Create<Guid>();
            var dbTask = _fixture.Create<DbTask>();

            var expected = new TaskDomain
            {
                Priority = dbTask.Priority,
                State = dbTask.State,
                AddedDate = dbTask.AddedDate,
                DueDate = dbTask.DueDate,
                Description = dbTask.Description,
                Id = dbTask.Id
            };

            _dbRepository.Setup(x=>x.GetByIdAsync(newGuid)).Returns(Task.FromResult(dbTask)).Verifiable();
            var actual = await _tasksRepository.GetAsync(newGuid);

            actual.Should().BeEquivalentTo(expected);
            _dbRepository.VerifyAll();

        }


        [Fact]
        public async void DeleteAsync_TaskDeleted()
        {
            var newGuid = _fixture.Create<Guid>();
            var dbTask = _fixture.Create<DbTask>();
           
            _dbRepository.Setup(x => x.GetByIdAsync(newGuid)).Returns(Task.FromResult(dbTask)).Verifiable();
            _dbRepository.Setup(x => x.Delete(dbTask)).Verifiable();

             await _tasksRepository.DeleteAsync(newGuid);

            _dbRepository.VerifyAll();
        }

        [Fact]
        public async void UpdateAsync_TaskUpdated()
        { 
            var taskDomain = _fixture.Create<TaskDomain>();
            var dbTask = _fixture.Create<DbTask>();

            _dbRepository.Setup(x => x.GetByIdAsync(taskDomain.Id)).Returns(Task.FromResult(dbTask)).Verifiable();

            await _tasksRepository.UpdateAsync(taskDomain);

            dbTask.Should().BeEquivalentTo(new DbTask
            {
                State = taskDomain.State,
                DueDate = taskDomain.DueDate,
                Description = taskDomain.Description,
                Priority = taskDomain.Priority,
                AddedDate = dbTask.AddedDate,
                Id = dbTask.Id
            });

            _dbRepository.VerifyAll();
        }
    }
}
