using AppWebSpa.Services;

namespace AppWebSpa.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public SeedDb(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task SeedAsync() {

            await new UserRolesSeeder(_context, _userService).SeedAsync();

        }
    }
}
