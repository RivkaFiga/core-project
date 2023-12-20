using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;
using Task1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Task1.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;


namespace Task1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskManagerController: ControllerBase
    {
        IUserService UserService;
        public TaskManagerController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        [HttpPost]

        public ActionResult<String> Login([FromBody] User User)
        {
            var dt = DateTime.Now;
            var user = this.UserService.GetAll().FirstOrDefault(u =>
                u.UserName == User.UserName 
                && u.Password == User.Password
            );        

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "TaskManager" : "TaskUser"),
                new Claim("userId", user.UserId.ToString()),
            };

            if(user.TaskManager)
                claims.Add(new Claim("UserType","TaskUser"));

            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}