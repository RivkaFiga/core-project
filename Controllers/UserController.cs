using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;
using Task1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.IO;
using Task1.Services;
// using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
// using.Microsoft.AspNetCore.Http;

namespace Task1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly long userId;
        IUserService UserService;
        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
            // this.userId = long.Parse(httpContextAccesor.httpConttext?.User?.FindFirst("userId")?.Value);
        }

        [HttpGet]
        [Authorize(Policy = "TaskManager")]
        public ActionResult<List<User>> GetAll() =>UserService.GetAll();

        [HttpGet("{id}")]
        [Authorize(Policy = "TaskUser")]
        public ActionResult<User> Get(long userId)
        {
            var getUser = UserService.Get(userId);

            if (getUser == null)
                return NotFound();

            return getUser;
        }

        [HttpPost] 
        [Authorize(Policy = "TaskManager")]
        public IActionResult Create(User user)
        {
            UserService.Add(user);
            return CreatedAtAction(nameof(Create), new{id=user.UserId}, user);
            //  return CreatedAtAction(nameof(Create), new {id=task.Id}, task);
        }

        // [HttpPut("{id}")]
        // [Authorize(Policy = "TaskUser")]
        // public IActionResult Update(User user)
        // {
        //     if (userId != user.UserId)
        //         return BadRequest();

        //     var existingUser = UserService.Get(userId);
        //     if (existingUser is null)
        //         return  NotFound();

        //     UserService.Update(user);

        //     return NoContent();
        // }

        [HttpDelete("{id}")]
        [Authorize(Policy = "TaskManager")]
        public IActionResult Delete(long userId)
        {
            var user = UserService.Get(userId);
            if (user is null)
                return  NotFound();

            UserService.Delete(userId);

            return Content(UserService.Count().ToString());
        }
    }
}