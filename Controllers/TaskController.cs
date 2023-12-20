using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;
using Task1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
// using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Task1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "TaskUser")]
    public class TaskController : ControllerBase
    {
                
        private readonly long userId;
        ITaskService TaskService;
        public TaskController(ITaskService TaskService,IHttpContextAccessor httpContextAccessor)
        {
            this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);
            this.TaskService = TaskService;
        }

        [HttpGet]
        public ActionResult<List<Task>> GetAll() =>TaskService.GetAll( userId);

        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var task = TaskService.Get(userId,id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost] 
        public IActionResult Create(Task task)
        {
            TaskService.Add(userId,task);
            return CreatedAtAction(nameof(Create), new {id=task.Id}, task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest();
            var existingTask = TaskService.Get(userId,id);
            if (existingTask is null)
                return  NotFound();
            TaskService.Update(userId,task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TaskService.Get(userId,id);
            if (task is null)
                return  NotFound();
            TaskService.Delete(userId,id);
            return Content(TaskService.Count(userId).ToString());
        }
    }
}