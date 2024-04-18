using WebProject.Models;

namespace WebProject.interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> Add(User user);
        public Task<List<User>?> GetByEmail(string email);
        public Task DeleteByEmail(string email);
        public Task<List<User>?> GetAllUsers();

        public Task EditUser(User user);
    }
}
