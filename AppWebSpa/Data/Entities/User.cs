using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWebSpa.Data.Entities
{
    public class User : IdentityUser
    {
        //Properties with DataAnnotations
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar un número de teléfono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de nacimiento")]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Password { get; set; }

        public int IdRol { get; set; } = 0; // rol que por determinado es 0 que es usuario.  Al administrador, a nivel de base de datos nosotros lo modificamos y le asignamos el 1 que es ADMIN
        // y ya este podra asignar los demas roles que cree.
    }
}
