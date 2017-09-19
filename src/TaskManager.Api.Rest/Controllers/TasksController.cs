using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.Tasks.Models;
using TaskManager.RegistryCommon;

namespace TaskManager.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        //todo: настроить мапер TaskDomain в DTO

        [HttpPost]
        [Route("GetTasks")]
        public async Task<TableResponseCommon<TaskDomain>> GetTasks(TaskSearchParams searchParams)
        {
            return await _taskService.SelectTasksAsync(searchParams).ConfigureAwait(true);
        }
        
        [HttpGet("{id}")]
        public async Task<TaskDomain> Get(Guid id)
        {
            return await _taskService.GetAsync(id).ConfigureAwait(true);
        }

        [HttpGet("{id}")]
        [Route("Cancel")]
        public async Task<TaskDomain> Cancel(Guid id)
        {
            return await _taskService.CancelAsync(id).ConfigureAwait(true);
        }
        
        [HttpPost]
        public async void Post([FromBody] TaskDomain model)
        {
            await _taskService.AddAsync(model).ConfigureAwait(true);
        }
        
        [HttpDelete("{id}")]
        public async void Delete(Guid id)
        {
            await _taskService.DeleteAsync(id).ConfigureAwait(true);
        }

        [HttpPut("{id}")]
        public async Task<TaskDomain> Put(Guid id, [FromBody] DateTime newDate)
        {
            return await _taskService.ReScheduleAsync(id, newDate).ConfigureAwait(true);
        }
    }
}
