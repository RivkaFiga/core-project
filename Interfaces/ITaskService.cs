using Task1.Models;
using System.Collections.Generic;

namespace Task1.Interfaces
{
    public interface ITaskService
    {
        List<Task> GetAll(long userId);
        Task Get( long userId,int id);
        void Add(long userId,Task task);
        void Delete(long userId,int id);
        void Update(long userId,Task task);
        int Count(long userId);
    }
}