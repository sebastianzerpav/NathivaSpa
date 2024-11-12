using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        public string Email { get; set; } = null!;

        [Display(Name = "Contraseña")]
        [MinLength(4, ErrorMessage = "El campo '{0}' debe tener al menos {1} caractéres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido")]
        public string Password { get; set; } = null!;
    }
}
