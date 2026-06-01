using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dto.UserTask;
using ToDoList.Application.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskService _service;

        public UserTaskController(IUserTaskService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserTask([FromBody] CreateUserTaskDto createDto, CancellationToken cancellationToken)
        {
            var userTask = await _service.CreateUserTaskAsync(createDto, cancellationToken);

            return CreatedAtAction(nameof(GetUserTaskById), new { id = userTask.Id }, userTask);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTaskById(Guid id, CancellationToken cancellationToken)
        {
            var taskUser = await _service.GetUserTaskByIdAsync(id, cancellationToken);

            return Ok(taskUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            var taskUser = await _service.DeleteUserTaskAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserTaskAsync(Guid id,[FromBody] UpdateUserTaskDto updateUserTaskDto, CancellationToken cancellationToken)
        {
            var taskUser= await _service.UpdateUserTaskAsync(id, updateUserTaskDto, cancellationToken);

            return Ok(taskUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTasksAsync([FromQuery] UserTaskFilter filter,[FromQuery] SortParams sortParams,[FromQuery] PageParams pageParams, CancellationToken cancellationToken)
        {
            var tasks = await _service.GetUsersAsync(filter,sortParams,pageParams, cancellationToken);

            return Ok(tasks);
        }

    }
}
