using AppWebSpa.Core.Pagination;
using AppWebSpa.Data.Entities;

namespace AppWebSpa.DTOs
{
    public class CategoryDTO : Category
    {
        public PaginationResponse<SpaService> PaginatedCategories { get; set; }
    }
}
