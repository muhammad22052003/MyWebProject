using WebProject.interfaces;
using WebProject.interfaces.Repositories;
using WebProject.Models;

namespace WebProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDBService _dbService;
        public UserRepository(IDBService dbService)
        {
            _dbService = dbService;
        }

        public async Task<bool> Add(User user)
        {
            var users = await _dbService.GetData<User>("users", $"email = \"{user.Email}\"");

            if(users.Count >= 1) return false;

            await _dbService.AddData("users", user);

            return true;
        }

        public async Task DeleteByEmail(string email)
        {
            List<User>? users = (await _dbService.GetData<User>("users", $"email = \"{email}\""))?.Cast<User>().ToList();

            IModel? user = users.Find(x => x.Email == email);

            if(user != null)
            {
                await _dbService.DeleteData("users", user);
            }
        }

        async public Task<List<User>?> GetByEmail(string email)
        {
            List<User>? users = (await _dbService.GetData<User>("users", $"email = \"{email}\""))?.Cast<User>().ToList();

            return users;
        }

        async public Task<List<User>?> GetAllUsers()
        {
            List<User>? users = (await _dbService.GetData<User>("users"))?.Cast<User>().ToList();

            return users;
        }

        async public Task EditUser(User user)
        {
            await _dbService.EditData(user, "users");
        }
    }
}
