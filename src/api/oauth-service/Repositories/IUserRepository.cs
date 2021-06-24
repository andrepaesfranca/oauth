using oauth_service.Models;
using System.Collections.Generic;

namespace oauth_service.Repositories
{
    public interface IUserRepository
    {
        void Create(User user);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email, string password);
        void Update(User user);
    }
}
