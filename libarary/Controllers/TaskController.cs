using libarary.Models;
using libarary.Services;
using Microsoft.AspNetCore.Mvc;

namespace libarary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // Change constructor to accept ITaskService, not DbTaskService
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("add")]
        public IActionResult AddTask([FromBody] Todo task)
        {
            try
            {
                var createdTask = _taskService.AddTodo(task);
                return Ok(createdTask);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{bookId}")]
        public IActionResult DeleteTask(int bookId)
        {
            try
            {
                _taskService.DeleteById(bookId);
                return Ok("Task deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{bookId}")]
        public IActionResult GetTask(int bookId)
        {
            var task = _taskService.GetTodoById(bookId);
            if (task == null)
                return NotFound("Task not found.");

            return Ok(task);
        }

        [HttpGet("all")]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskService.GetTodos();
            return Ok(tasks);
        }

        [HttpPut("update/{bookId}")]
        public IActionResult UpdateTask(int bookId, [FromBody] Todo updatedTask)
        {
            var updatedTodo = _taskService.UpdateById(bookId, updatedTask);
            if (updatedTodo == null)
                return NotFound("Task not found.");

            return Ok(updatedTodo);
        }

        // Buy a book
        [HttpPost("{bookId}/buy")]
        public IActionResult BuyBook(int bookId)
        {
            try
            {
                _taskService.BuyBook(bookId);
                return Ok("Book purchased successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Reserve a book
        [HttpPost("{bookId}/reserve")]
        public IActionResult ReserveBook(int bookId)
        {
            try
            {
                _taskService.ReserveBook(bookId);
                return Ok("Book reserved successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cancel a reservation
        [HttpPost("{bookId}/cancel")]
        public IActionResult CancelReservation(int bookId)
        {
            try
            {
                _taskService.CancelReservation(bookId);
                return Ok("Reservation cancelled successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
