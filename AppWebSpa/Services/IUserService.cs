using Microsoft.AspNetCore.Identity;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using AppWebSpa.Data;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public Task<IdentityResult> ConfirmEmail(User user, string token);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);

        //public Task<User> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();

    }

    public class UsersService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UsersService(DataContext dataContext, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmail(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        //public async Task<User> GetUserAsync(string email)
        //{
        //    User? user = 
        //        await _dataContext.User.Include(
        //        u => u.Role).FirstOrDefaultAsync(
        //        u => u.Email == email);

        //    return user;
        //}

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
