using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dto;
using ToDoList.Application.Interfaces;

namespace ToDoList.API.Controllers
{
    [Route("controller/[controller]")]
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

            if (!taskUser)
            {
                return BadRequest(new {message = "UserTask not found" });
            }

            return Ok(taskUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserTaskAsync(Guid id,[FromBody] UpdateUserTaskDto updateUserTaskDto, CancellationToken cancellationToken)
        {
            var taskUser= await _service.UpdateUserTaskAsync(id, updateUserTaskDto, cancellationToken);

            if (taskUser == null)
            {
                return BadRequest(new { message = "UserTask not found" });
            } 

            return Ok(taskUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersTasksAsync(CancellationToken cancellationToken)
        {
            var tasks = await _service.GetUsersAsync(cancellationToken);

            return Ok(tasks);
        }

    }
}
