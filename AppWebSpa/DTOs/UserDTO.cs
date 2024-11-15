using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.DTOs
{
    public class UserDTO
    {
        //Guid: cadena generada por 4 secciones de 4 digitos separadas por un guion
        public Guid Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} carateres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = null!;

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateOnly BirthDate { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "el campo {0} debe ser un email valido")]
        public string Email { get; set; } = null!;

        public IEnumerable<SelectListItem>? NathivaRoles { get; set; }=null!;

        public int NathivaRoleId { get; set; } = 0; 
    }
}
