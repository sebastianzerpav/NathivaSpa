using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWebSpa.Data.Entities
{
    public class User : IdentityUser
    {

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Debe ingresar una fecha de nacimiento")]
        public DateOnly BirthDate { get; set; }


        public int IdRol { get; set; } = 0; // rol que por determinado es 0 que es usuario.  Al administrador, a nivel de base de datos nosotros lo modificamos y le asignamos el 1 que es ADMIN
        // y ya este podra asignar los demas roles que cree.
    }
}
