using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebProject.interfaces.auth;
using WebProject.interfaces.Repositories;
using WebProject.Models;
using WebProject.Models.Requests;
using WebProject.Provaiders;

namespace WebProject.Services
{
    public class UserService
    {
        private readonly ICustomPasswordHasher _passwordHasher;

        private readonly IJwtProvider _jwtProvider;

        private readonly IUserRepository _userRepository;

        public UserService
        (
            ICustomPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            IUserRepository userRepository
        )
        {
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
        }

        async public Task<bool> Registration(UserRegistrationRequest request)
        {
            string hashedPassword = _passwordHasher.Generate(request.Password);

            User user = User.Create(request.Name, hashedPassword, request.Email.ToLower());
            user.Role = "admin";
            user.RegistrationTime = DateTime.Now;
            user.LastLoginTime = DateTime.Now;

            return await _userRepository.Add(user);
        }

        public async Task<string?> Login(UserLoginRequest request)
        {
            var users = await _userRepository.GetByEmail(request.Email.ToLower());

            User? user = users.Find((x) => x.Email == request.Email.ToLower());

            if(user == null || user.isBlocked) { return null; }

            bool result = _passwordHasher.Verify(request.Password, user.HashPassword);

            if (!result)
            {
                return null;
            }

            user.LastLoginTime = DateTime.Now;

            await _userRepository.EditUser(user);

            string token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<List<User>?> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public bool CheckToken(string token, User user)
        {
            var result = _jwtProvider.DecodeJwtToken(token);

            if(user.Email == result.Email && user.Id == result.UserId)
            {
                return true;
            }

            return false;
        }

        public async Task<User?> GetTokenUser(string token)
        {
            var result = _jwtProvider.DecodeJwtToken(token);

            List<User>? Users = await _userRepository.GetByEmail(result.Email);

            if (Users.IsNullOrEmpty()) {  return null; }

            return Users.FirstOrDefault(x => x.Email == result.Email);
        }

        async public Task EditUser(User user)
        {
            await _userRepository.EditUser(user);
        }

        async public Task DeletUser(string email)
        {
            await _userRepository.DeleteByEmail(email);
        }
    }
}
