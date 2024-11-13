using AppWebSpa.Data.Entities;

namespace AppWebSpa.Models
{
    public class AssignRoleViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<RolesForUser> Roles { get; set; } = new List<RolesForUser>();
    }
}
