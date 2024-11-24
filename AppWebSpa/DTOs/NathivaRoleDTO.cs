using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.DTOs
{
    public class NathivaRoleDTO
    {
        //Properties with DataAnnotations
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Debe ingresar un nombre para el rol")]
        public string Name { get; set; } = null!;

        
        public List<PermissionForDTO>? Permissions { get; set; }
        public List<CategoryForDTO>? Categories { get; set; }

        public string? PermissionIds { get; set; }

        public string? CategoryIds { get; set; }


    }
}
