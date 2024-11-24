namespace AppWebSpa.Data.Entities
{
    public class RoleCategory
    {
        
        public int RoleId { get; set; }
        public NathivaRole Role { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
