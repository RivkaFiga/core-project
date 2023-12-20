using Task1.Models;
using Task1.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Task1.Services
{
  //  using Microsoft.AspNetCore.Http;

    public class TaskService : ITaskService
    {
        List<Task> Tasks { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;

        public TaskService(IWebHostEnvironment webHost)
        {

            // System.Console.WriteLine(httpContextAccessor.httpContext);
            // this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        public List<Task> GetAll(long userId) 
        {
            return Tasks.Where(t => t.UserId == userId).ToList();
        }

        public Task Get(long userId,int id)
        {
            return Tasks.FirstOrDefault(t => t.UserId == userId && t.Id == id);
        }

        public void Add(long userId,Task task)
        {
            task.Id = Tasks.Count() + 1;
            task.UserId = userId;
            Tasks.Add( task);
            saveToFile();
        }

        public void Delete(long userId,int id)
        {
            var task = Get( userId,id);
            if (task is null)
                return;
            Tasks.Remove(task);
            saveToFile();
        }

        public void Update(long userId,Task task)
        {
            var index = Tasks.FindIndex(t => t.UserId == userId &&  t.Id == task.Id);
            if (index == -1)
                return;
            task.UserId=userId;
            task.Id=Count( userId)+1;
            Tasks[index] = task;
            saveToFile();
        }

        public int Count(long userId) 
        { 
            return GetAll( userId).Count();
        }
    }
}