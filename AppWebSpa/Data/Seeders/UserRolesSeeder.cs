using AppWebSpa.Data.Entities;
using AppWebSpa.Services;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public UserRolesSeeder(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task SeedAsync() {
            //await CheckRoles();
            await CheckUsers();
        }
        private async Task CheckUsers()
        {
            User? user = await _userService.GetUserAsync((await _context.User.FirstOrDefaultAsync())?.Email);
            //(await _context.User.FirstOrDefaultAsync())?.Email
            //User? user = await _context.User.FirstOrDefault();


            if (user == null)
            {
                user = new User
                {
                    //Email = "admin@yopmail.com",
                    //UserName = "admin@yopmail.com",
                    //Name = "Admin",


                    Email = "manuel@yopmail.com",
                    Name = "Manuel",
                    PhoneNumber = "30000000",
                    UserName = "manuel@yopmail.com",
                };

                var result = await _userService.AddUserAsync(user, "admin");

                string token = await _userService.GenerateEmailConfirmationTokenAsync(user);
                await _userService.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRoles()
        {
            throw new NotImplementedException();
        }
    }
}
