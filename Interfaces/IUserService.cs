using Task1.Models;
using System.Collections.Generic;

namespace Task1.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User Get(long userId);
        void Post(User user);
        void Add(User user);
        void Delete(long id);
        void Update(User user);
        int Count();

    } 
    
}