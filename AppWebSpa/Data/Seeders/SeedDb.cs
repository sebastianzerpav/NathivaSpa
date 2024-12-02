using AppWebSpa.Services;

namespace AppWebSpa.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersService _userService;

        public SeedDb(DataContext context, IUsersService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task SeedAsync() 
        {
            await new CategoriesSeeder(_context).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _userService).SeedAsync();

        }
    }
}
