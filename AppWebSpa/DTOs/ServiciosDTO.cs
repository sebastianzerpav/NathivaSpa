using AppWebSpa.Core.Pagination;
using AppWebSpa.Data.Entities;

namespace AppWebSpa.DTOs
{
    public class ServiciosDTO : Category
    {

        public PaginationResponse<Category> paginatedCategories { get; set; }
    }
}
