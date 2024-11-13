using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.DTOs
{
    public class RolesDTO
    {
        //Properties with DataAnnotations
        [Key]
        public int IdRol { get; set; }

        [Display(Name = "Nombre del rol")]
        [Required(ErrorMessage = "Debe ingresar un nombre para el rol")]
        public string Name { get; set; }

        public int State { get; set; } = 1;

    }
}
