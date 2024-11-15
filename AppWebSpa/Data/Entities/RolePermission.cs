namespace AppWebSpa.Data.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public NathivaRole Role { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
