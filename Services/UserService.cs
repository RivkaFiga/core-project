using Task1.Models;
using Task1.Interfaces;
using Task1.Services;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;
// using.Microsoft.AspNetCore.Http;


namespace Task1.Services
{
    public class UserService : IUserService
    {
        List<User> users { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        ITaskService iTaskService;
        public UserService(IWebHostEnvironment webHost,ITaskService iTaskService)
        {
            this.iTaskService = iTaskService;
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "user.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }
        
        public List<User> GetAll()=>users; 
        
        public User Get(long userId)=>users?.FirstOrDefault(u => u.UserId == userId);
        
        public void Post(User user){
            user.UserId=users[users.Count()-1].UserId+1;
            users.Add(user);
            saveToFile();
        }

        public void Update(User user)
        {
            var index = users.FindIndex(u => u.UserId == user.UserId );
            if (index == -1)
                return;
            users[index] = user;
            saveToFile();
        }

        public void Delete(long userId)
        {
            var user = Get(userId);
            if (user is null)
                return;
            users.Remove(user);
            List<Task> tasks = iTaskService.GetAll(userId);
            foreach (var t in tasks)
                iTaskService.Delete(userId,t.Id);
            saveToFile();
        }

        public void Add(User user)
        {
            user.UserId = users.Count() + 1;
            users.Add(user);
            saveToFile();
        }
        public int Count() 
        { 
            return GetAll().Count();
        }


    }
    
}