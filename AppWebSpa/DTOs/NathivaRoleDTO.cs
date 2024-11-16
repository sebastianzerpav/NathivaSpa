using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.DTOs
{
    public class NathivaRoleDTO
    {
        //Properties with DataAnnotations
        public int Id { get; set; }

        [Display(Name = "Nombre del rol")]
        [Required(ErrorMessage = "Debe ingresar un nombre para el rol")]
        public string Name { get; set; }

        
        public List<PermissionForDTO>? Permissions { get; set; }

        public string? PermissionIds { get; set; }


    }
}
