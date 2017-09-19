using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using TaskManager.DataLayer.Tasks;
using TaskManager.DataLayer.TransactionManagement;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.Tasks.Models;
using TaskManager.Domain.TasksImpl;
using TaskManager.RegistryCommon;
using Xunit;

namespace TaskManager.Domail.Tests
{ 
    public class TaskServiceTests
    {
        private readonly Mock<ITasksRepository> _repository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ITaskStateMachine> _taskStateMachine;
        private readonly Fixture _fixture;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _repository = new Mock<ITasksRepository>(MockBehavior.Strict);
            _unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _taskStateMachine = new Mock<ITaskStateMachine>(MockBehavior.Strict);
            _fixture = new Fixture();
            _taskService = new TaskService(_repository.Object, _unitOfWork.Object, _taskStateMachine.Object);
        }

        [Fact]
        public async void AddAsync_TaskAdded()
        {
            var task = _fixture.Create<TaskDomain>();
            _repository.Setup(x => x.AddAsync(task)).Returns(Task.FromResult(false)).Verifiable();
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(false)).Verifiable();

            await _taskService.AddAsync(task);

            _repository.VerifyAll();
            _unitOfWork.VerifyAll();
        }

        [Fact]
        public async void GetAsync_ReturnsTask()
        {
            var task = _fixture.Create<TaskDomain>();
            var taskId = _fixture.Create<TaskRef>();

            _repository.Setup(x => x.GetAsync(taskId)).Returns(Task.FromResult(task)).Verifiable();

            var actual = await _taskService.GetAsync(taskId);
            actual.Should().BeEquivalentTo(task);

            _repository.VerifyAll(); 
        }

        [Fact]
        public async void DeleteAsync_TaskDeleted()
        {
            var taskId = _fixture.Create<TaskRef>();

            _repository.Setup(x => x.DeleteAsync(taskId)).Returns(Task.FromResult(false)).Verifiable();
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(false)).Verifiable();

            await _taskService.DeleteAsync(taskId);

            _repository.VerifyAll();
            _unitOfWork.VerifyAll();
        }

        [Fact]
        public async void SelectTasksAsync_ReturnsTasks()
        {
            var expected = _fixture.Create<TableResponseCommon<TaskDomain>>();
            var searchParams = _fixture.Create<TaskSearchParams>();

            _repository.Setup(x => x.SelectTasksAsync(searchParams)).Returns(Task.FromResult(expected)).Verifiable();

            var actual = await _taskService.SelectTasksAsync(searchParams);
            actual.Should().BeEquivalentTo(expected);

            _repository.VerifyAll();
        }

        [Fact]
        public async void ReScheduleAsync_TaskChanged()
        {
            var taskId = _fixture.Create<TaskRef>();
            var newDate = _fixture.Create<DateTime>();
            var task = _fixture.Create<TaskDomain>();
            task.DueDate = newDate;

            _repository.Setup(x => x.GetAsync(taskId)).Returns(Task.FromResult(task)).Verifiable();
            _repository.Setup(x => x.UpdateAsync(task)).Returns(Task.FromResult(task)).Verifiable();
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(false)).Verifiable();

            var actual = await _taskService.ReScheduleAsync(taskId, newDate);
            actual.DueDate.Should().Be(newDate);

            _repository.VerifyAll();
            _unitOfWork.VerifyAll();
        }

        [Fact]
        public async void CancelAsync_TaskStateChanged()
        {
            var taskId = _fixture.Create<TaskRef>();
            var newState = TaskState.Canceled;
            var task = _fixture.Create<TaskDomain>();

            _repository.Setup(x => x.GetAsync(taskId)).Returns(Task.FromResult(task)).Verifiable();
            _taskStateMachine.Setup(x=>x.ValidateSwitch(task.State ,newState)).Verifiable();
            _repository.Setup(x => x.UpdateAsync(task)).Returns(Task.FromResult(task)).Verifiable();
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(false)).Verifiable();

            var actual = await _taskService.CancelAsync(taskId);
            actual.State.Should().Be(newState);

            _repository.VerifyAll();
            _unitOfWork.VerifyAll();
            _taskStateMachine.VerifyAll();
        }
    }
}
