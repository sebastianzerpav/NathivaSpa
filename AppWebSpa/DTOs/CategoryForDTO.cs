using AppWebSpa.Data.Entities;

namespace AppWebSpa.DTOs
{
    public class CategoryForDTO: Category
    {
        public bool Selected { get; set; } = false;
    }
}